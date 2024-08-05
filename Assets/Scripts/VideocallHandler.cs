using Agora.Rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class VideocallHandler : MonoBehaviour
{
    [SerializeField] public List<VideoSurface> remoteUsers { get; private set; } = new List<VideoSurface>();

    [SerializeField] private VideocallsGridView _videocallsGridView;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };

#endif

    private IRtcEngine _rtcEngine;

    private void OnEnable()
    {
        AgoraCallbackHandler.RemoteUserJoined += RemoteUserJoinedHandler;

    }
    private void OnDisable()
    {
        AgoraCallbackHandler.RemoteUserJoined -= RemoteUserJoinedHandler;

    }

    private void Start()
    {
        SetupEngine();

        AgoraCallbackHandler handler = new AgoraCallbackHandler();

        _rtcEngine.InitEventHandler(handler);
        
    }

    void OnApplicationQuit()
    {
        if (_rtcEngine != null)
        {
            Leave();
            // Destroy IRtcEngine
            _rtcEngine.Dispose();
            _rtcEngine = null;
        }
    }

    public void Leave()
    {
        Debug.Log($"Leaving ");
        // Leave the channel
        _rtcEngine.LeaveChannel();
        // Disable the video module
        _rtcEngine.DisableVideo();
        // Stop remote video rendering

        foreach(var userVideo in remoteUsers)
        {
            userVideo.SetEnable(false);
        }

        // Stop local video rendering
        _videocallsGridView.GetLocalFrame().GetComponent<VideoSurface>().SetEnable(false);
    }

    private void SetupEngine()
    {
        // Create an IRtcEngine instance
        _rtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = AgoraConf.AppID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // Initialize the instance
        _rtcEngine.Initialize(context);
    }
    public void Join()
    {
        if (_videocallsGridView == null)
        {
            Debug.LogError("Videocalls grid view not found, please assign it in inspector");
            return;
        }

        GameObject localFrameView = _videocallsGridView.GetLocalFrame();
        ChannelMediaOptions options = new ChannelMediaOptions();

        Debug.Log("Try join...");
        try
        {
            _rtcEngine.EnableVideo();
            if (localFrameView.TryGetComponent(out VideoSurface videoSurface))
            {
                //localFrameView.transform.Rotate(0.0f, 0.0f, -180.0f);

                videoSurface.SetForUser(0, "");
                videoSurface.SetEnable(true);
                options.autoSubscribeAudio.SetValue(true);
                options.autoSubscribeVideo.SetValue(true);
                options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
                options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
                _rtcEngine.JoinChannel(AgoraConf.Token, AgoraConf.ChannelName, 0, options);

                Debug.Log($"Local user joined");

            }
            else
            {
                Debug.LogError("Video surface component not found");
            }
        }
        catch(Exception e) 
        {
            Debug.LogError("Error during user join:" + e.Message, this);
            
        }

    }
    
    private void RemoteUserJoinedHandler(UserCallbackEventArgs eventArgs)
    {
        Debug.Log("Remote user try join..", this);
        if (_videocallsGridView == null)
        {
            Debug.LogError("Videocalls grid view not found, please assign it in inspector");
            return;
        }

        GameObject frameView = _videocallsGridView.AddRemoteFrame();
        if (frameView == null)
        {
            Debug.LogError("Error during frame view instantiation");
            return;
        }

        if (frameView.TryGetComponent(out VideoSurface videoSurface))
        {
            videoSurface.SetForUser(eventArgs.uid, eventArgs.connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            videoSurface.SetEnable(true);
            remoteUsers.Add(videoSurface);
            Debug.Log($"Remote user with id:{eventArgs.uid} joined");
        }
        else
        {
            Debug.LogError("Video surface component not found");
        }
    }
    
}
