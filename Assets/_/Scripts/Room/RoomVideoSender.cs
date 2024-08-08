using LiveKit;
using LiveKit.Proto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomVideoSender : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _requestedFPS = default;
    [SerializeField] private Vector2 _textureSize = default;

    [Header("References")]
    [SerializeField] private RawImage _rawImage = default;
    [Space]
    [SerializeField] private WebCamDevice _webcamDevice = default;
    [SerializeField] private List<WebCamDevice> _webcamDevices = default;

    private RoomUser _user = default;
    private WebCamTexture _webcamTexture = default;

    protected override void Inizialize()
    {

        _webcamTexture = new WebCamTexture();
        _webcamDevices = WebCamTexture.devices.ToList();

        Debug.Log($"[RoomVideoSender] ~ [Inizialize] - Find Out {_webcamDevices.Count} Devices.");

        foreach (WebCamDevice device in _webcamDevices)
        {
            Debug.Log($"[RoomVideoSender] ~ [Inizialize] - Device N.{_webcamDevices.IndexOf(device)} Is {device.name}.");
        }

    }
    protected override void Dispose()
    {

        _webcamDevices = null;
        _webcamTexture = null;
    
    }

    private void SelectDevice(int index)
    {

        if (index >= _webcamDevices.Count)
        {
            Debug.Log($"[RoomVideSender] - [SelectDevice] ~ Error Selecting Webcam Device At Index {index}.");
            return;
        }

        _webcamDevice = _webcamDevices[index];

        if (string.IsNullOrEmpty(_webcamDevice.name))
        {
            Debug.Log($"[RoomVideSender] - [SelectDevice] ~ Error Selecting Webcam Device At Index {index}.");
            return;
        }

        Debug.Log($"[RoomVideSender] - [SelectDevice] ~ Selecting Webcam Device {_webcamDevice.name} Of Type {_webcamDevice.kind} At Index {index}.");

        _webcamTexture = new WebCamTexture(_webcamDevice.name, (int)_textureSize.x, (int)_textureSize.y, _requestedFPS);
        _rawImage.texture = _webcamTexture;
        _webcamTexture.Play();

    }

    private IEnumerator CreateLocalTrack()
    {

        TextureVideoSource source = new TextureVideoSource(_rawImage.texture);
        LocalVideoTrack track = LocalVideoTrack.CreateVideoTrack("tester-local-video", source, RoomSession.Room);

        TrackPublishOptions options = new TrackPublishOptions();
        options.VideoCodec = VideoCodec.Vp8;
        var videoCoding = new VideoEncoding();
        videoCoding.MaxBitrate = 512000;
        videoCoding.MaxFramerate = _requestedFPS;
        options.VideoEncoding = videoCoding;
        options.Simulcast = true;
        options.Source = TrackSource.SourceCamera;

        var publish = _user.GetLocalParticipant().PublishTrack(track, options);
        yield return publish;

        if (!publish.IsError)
        {
            Debug.Log("Track published!");
            yield break;
        }

        source.Start();
        StartCoroutine(source.Update());

    }

    void TrackSubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {

        if (track is RemoteVideoTrack videoTrack)
        {
            var stream = new VideoStream(videoTrack);
            stream.TextureReceived += (tex) =>
            {
                _rawImage.texture = tex;
            };
            StartCoroutine(stream.Update());
        }

    }

}