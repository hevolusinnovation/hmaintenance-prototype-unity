using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;
using UnityEngine.UI;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

using Agora.Rtc;


public class JoinChannel : MonoBehaviour
{

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
    
#endif

    // Fill in your app ID
    private string _appID = "f6c0453777d846daa4423fbb663e6859";
    // Fill in your channel name
    private string _channelName = "Unity_Channel";
    // Fill in a temporary token
    private string _token = "007eJxTYDBh2v3j6q/sXc/m9kWw/zA0m6vB4vbIZO2Zjd7nYjzPHJBWYEgzSzYwMTU2NzdPsTAxS0lMNDExMk5LSjIzM041szC13Fy/Jq0hkJFBh+MCAyMUgvi8DKF5mSWV8c4ZiXl5qTkMDAC5ZiNC";
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;


    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        Debug.Log("UNITY_2018_3_OR_NEWER && UNITY_ANDROID");
#endif
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
    }

// Update is called once per frame
void Update()
    {
        CheckPermissions();

    }

    public void Join()
    {
        Debug.Log("Try join...");
        // Enable the video module
        RtcEngine.EnableVideo();
        // Set channel media options
        ChannelMediaOptions options = new ChannelMediaOptions();
        // Start video rendering
        LocalView.SetEnable(true);
        // Automatically subscribe to all audio streams
        options.autoSubscribeAudio.SetValue(true);
        // Automatically subscribe to all video streams
        options.autoSubscribeVideo.SetValue(true);
        // Set the channel profile to live broadcast
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        //Set the user role as host
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Join a channel
        RtcEngine.JoinChannel(_token, _channelName, 0, options);

        Debug.Log("Joined");

    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("LocalView");
        LocalView = go.AddComponent<VideoSurface>();
        
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        Debug.Log("UI Setup OK");
    }

    public void Leave()
    {
        Debug.Log($"Leaving {_channelName}");
        // Leave the channel
        RtcEngine.LeaveChannel();
        // Disable the video module
        RtcEngine.DisableVideo();
        // Stop remote video rendering
        RemoteView.SetEnable(false);
        // Stop local video rendering
        LocalView.SetEnable(false);
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            // Destroy IRtcEngine
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    private void CheckPermissions()
    {
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
    }

    private void SetupVideoSDKEngine()
    {
        // Create an IRtcEngine instance
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = _appID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // Initialize the instance
        RtcEngine.Initialize(context);
    }

    // Create a user event handler instance and set it as the engine event handler
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    // Implement your own EventHandler class by inheriting the IRtcEngineEventHandler interface class implementation
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannel _videoSample;
        internal UserEventHandler(JoinChannel videoSample)
        {
            _videoSample = videoSample;
        }
        // error callback
        public override void OnError(int err, string msg)
        {
        }
        // Triggered when a local user successfully joins the channel
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            _videoSample.LocalView.SetForUser(0, "");
        }

        // When the SDK receives the first frame of a remote video stream and successfully decodes it, the OnUserJoined callback is triggered.
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // Set the remote video display
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Start video rendering
            _videoSample.RemoteView.SetEnable(true);
            Debug.Log("Remote user joined");
        }

        // This callback is triggered when a remote user leaves the current channel
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
        }

    }
}

