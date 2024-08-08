using Agora.Rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class VideocallHandler : MonoBehaviour
{
    [SerializeField] public List<CallFrameController> userFrames { get; private set; } = new List<CallFrameController>();

    [SerializeField] private VideocallsGridView _videocallsGridView;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };

#endif

    private IRtcEngine _rtcEngine;

    private void OnEnable()
    {
        AgoraCallbackHandler.RemoteUserJoined += RemoteUserJoinedHandler;
        AgoraCallbackHandler.RemoteUserLeft += RemoteUserLeftHandler;

    }
    private void OnDisable()
    {
        AgoraCallbackHandler.RemoteUserJoined -= RemoteUserJoinedHandler;
        AgoraCallbackHandler.RemoteUserLeft -= RemoteUserLeftHandler;

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

        foreach (var userFrame in userFrames)
        {
            userFrame.Destroy();
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

        CallFrameController localFrameView = _videocallsGridView.GetLocalFrame();
        ChannelMediaOptions options = new ChannelMediaOptions();

        Debug.Log("Try join...");
        try
        {
            _rtcEngine.EnableVideo();
            localFrameView.SetFrame(0, "");
            localFrameView.SetEnable(true);
            options.autoSubscribeAudio.SetValue(true);
            options.autoSubscribeVideo.SetValue(true);
            
            options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
            options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            SetVideoEncoderConfiguration();

            _rtcEngine.JoinChannel(AgoraConf.Token, AgoraConf.ChannelName, 0, options);

            Debug.Log($"Local user joined");
        }
        catch (Exception e)
        {
            Debug.LogError("Error during user join:" + e.Message, this);
        }

    }

    private void SetVideoEncoderConfiguration()
    {
        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            dimensions = new VideoDimensions(640, 360), // Risoluzione 640x360
            frameRate = 10, // Frame rate 10 fps
            bitrate = 400, // Bitrate 400 Kbps
            orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE
        };

        _rtcEngine.SetVideoEncoderConfiguration(config);
    }

    private void RemoteUserJoinedHandler(UserCallbackEventArgs eventArgs)
    {
        Debug.Log("Remote user try join..", this);
        if (_videocallsGridView == null)
        {
            Debug.LogError("Videocalls grid view not found, please assign it in inspector");
            return;
        }

        CallFrameController frame = _videocallsGridView.CreateFrame();
        if (frame == null)
        {
            Debug.LogError("Error during frame view instantiation");
            return;
        }

        frame.SetFrame(eventArgs.uid, eventArgs.connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        frame.SetEnable(true);
        userFrames.Add(frame);
        Debug.Log($"Remote user with id:{eventArgs.uid} joined");
    }

    private void RemoteUserLeftHandler(UserCallbackEventArgs eventArgs)
    {
        Debug.Log($"Remote user with id:{eventArgs.uid} left", this);
        if (_videocallsGridView == null)
        {
            Debug.LogError("Videocalls grid view not found, please assign it in inspector");
            return;
        }

        CallFrameController frame = userFrames.Where(f => f.id == eventArgs.uid).FirstOrDefault();
        if (frame == null)
        {
            Debug.LogError($"Error during frame retrieving id:{eventArgs.uid}", this);
            return;
        }

        userFrames.Remove(frame);
        frame.Destroy();
        Debug.Log($"Remote user with id:{eventArgs.uid} removed");
    }

    public void SwitchCamera()
    {
        Debug.Log("Switch camera");
        _rtcEngine.SwitchCamera();
        
    }


    public void StartScreenSharing()
    {
        ScreenCaptureParameters2 screenCaptureParameters2 = new ScreenCaptureParameters2();
        screenCaptureParameters2.captureVideo = true;
        screenCaptureParameters2.captureAudio = true;

        // Enable screen sharing.
        _rtcEngine.StartScreenCapture(screenCaptureParameters2);
        ChannelMediaOptions channelMediaOptions = new ChannelMediaOptions();
        channelMediaOptions.publishScreenCaptureVideo.SetValue(true);
        channelMediaOptions.publishScreenCaptureAudio.SetValue(true);
        channelMediaOptions.publishCameraTrack.SetValue(false);
        channelMediaOptions.publishMicrophoneTrack.SetValue(false);
        // Update channel media options.
        _rtcEngine.UpdateChannelMediaOptions(channelMediaOptions);
    }

    public void StopScreenCapture()
    {

        _rtcEngine.StopScreenCapture();
        //ChannelMediaOptions channelMediaOptions = new ChannelMediaOptions();
        //channelMediaOptions.publishScreenCaptureVideo.SetValue(true);
        //channelMediaOptions.publishScreenCaptureAudio.SetValue(true);
        //channelMediaOptions.publishCameraTrack.SetValue(false);
        //channelMediaOptions.publishMicrophoneTrack.SetValue(false);
        //// Update channel media options.
        //_rtcEngine.UpdateChannelMediaOptions(channelMediaOptions);
    }

}
