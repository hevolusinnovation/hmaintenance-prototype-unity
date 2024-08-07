using LiveKit;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class RoomVideoSender : RoomBehaviour
{
    [Header("Test")]
    [SerializeField] private Object _testObject = default;

    [Header("Settings")]
    [SerializeField] private bool _retryOnError = default;
    [Space]
    [SerializeField] private int _requestedFPS = default;
    [SerializeField] private Vector2 _textureSize = default;

    [Header("References")]
    [SerializeField] private WebCamDevice _webcamDevice = default;
    [Space]
    [SerializeField] private List<WebCamDevice> _webcamDevices = default;
    [Space]
    [SerializeField] private RoomUser _user = default;

    [Space] RawImage _rawImage = default;
    private WebCamTexture _webcamTexture = default;

    private Queue<Texture2D> _encodedImageData = new Queue<Texture2D>();
    //private Queue<LocalTrack> _encodedTracks = new Queue<LocalTrack>();

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

    private void Update()
    {
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

    [ContextMenu(nameof(PackData))]
    private async void PackData()
    {
        Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send.");

        //if (_object == null)
        //{
        //    Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send Is Null.");
        //    return;
        //}

        //BinaryFormatter bf = new BinaryFormatter();
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    bf.Serialize(ms, _object);
        //    _encodedPackData.Enqueue(ms.ToArray());
        //    Debug.Log($"[RoomDataSender] - [PackData] ~ Success: Encoded Pack Data.");
        //}
    }

    [ContextMenu(nameof(GetPackData))]
    private async void GetPackData()
    {
        Debug.Log($"[RoomDataSender] - [GetPackData] ~ Packing Data To Send.");

        HttpResponseMessage response = await APIUtility.GetFromEndpoint("http://localhost:7880");

        if (response == null)
        {
            Debug.Log($"[RoomDataSender] - [GetPackData] ~ Response Is Invalid.");
        }

        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                Debug.Log($"[RoomDataSender] - [GetPackData] ~ Response Code Is: 200.");
                break;
            case System.Net.HttpStatusCode.BadRequest:
                Debug.Log($"[RoomDataSender] - [GetPackData] ~ Response Code Is: 400.");
                break;
            case System.Net.HttpStatusCode.InternalServerError:
                Debug.Log($"[RoomDataSender] - [GetPackData] ~ Response Code Is: 500.");
                break;
            case System.Net.HttpStatusCode.BadGateway:
                Debug.Log($"[RoomDataSender] - [GetPackData] ~ Response Code Is: 502.");
                break;
        }
    }

    [ContextMenu(nameof(PostPackData))]
    private async void PostPackData()
    {
        Debug.Log($"[RoomDataSender] - [PostPackData] ~ Packing Data To Send.");

        string content = JsonConvert.SerializeObject(_testObject);
        HttpResponseMessage response = await APIUtility.PostToEndpoint(new StringContent(content), "http://localhost:7880");

        if (response == null)
        {
            Debug.Log($"[RoomDataSender] - [PostPackData] ~ Response Is Invalid.");
        }

        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                Debug.Log($"[RoomDataSender] - [PostPackData] ~ Response Code Is: 200.");
                break;
            case System.Net.HttpStatusCode.BadRequest:
                Debug.Log($"[RoomDataSender] - [PostPackData] ~ Response Code Is: 400.");
                break;
            case System.Net.HttpStatusCode.InternalServerError:
                Debug.Log($"[RoomDataSender] - [PostPackData] ~ Response Code Is: 500.");
                break;
            case System.Net.HttpStatusCode.BadGateway:
                Debug.Log($"[RoomDataSender] - [PostPackData] ~ Response Code Is: 502.");
                break;
        }
    }

    //private void SendData(LocalTrack localTrack)
    //{
    //    StartCoroutine(CO_SendData(localTrack));
    //}
    //private IEnumerator CO_SendData(LocalTrack localTrack)
    //{
    //    JSPromise<LocalTrackPublication> promise = _user.GetLocalParticipant().PublishTrack(localTrack);
    //    yield return promise;

    //    if (promise.IsError)
    //    {
    //        Debug.Log($"[RoomVideoSender] - [CO_SendData] ~ Publishing AudioData Operation Ends With Error.");

    //        if (_retryOnError)
    //        {
    //            yield return CO_SendData(localTrack);

    //            Debug.Log($"[RoomVideoSender] - [CO_SendData] ~ Retry Attemp To Publishing AudioData Operation Ends Correctly.");
    //            yield break;
    //        }

    //        yield break;
    //    }

    //    if (promise.IsDone)
    //    {
    //        Debug.Log($"[RoomVideoSender] - [CO_SendData] ~ Publishing AudioData Operation Ends Correctly.");
    //    }
    //}

    private void Test_Method()
    {
        // publish a WebCamera
        //var source = new TextureVideoSource(webCamTexture);

        // Publish the entire screen
        //var source = new ScreenVideoSource();

        // Publishing a Unity Camera
        //Camera.main.enabled = true;
        //var source = new CameraVideoSource(Camera.main);

        //var rt = new UnityEngine.RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        //rt.Create();
        //Camera.main.targetTexture = rt;
        //var source = new TextureVideoSource(rt);
        //var track = LocalVideoTrack.CreateVideoTrack("my-video-track", source, room);

        //var options = new TrackPublishOptions();
        //options.VideoCodec = VideoCodec.Vp8;
        //var videoCoding = new VideoEncoding();
        //videoCoding.MaxBitrate = 512000;
        //videoCoding.MaxFramerate = frameRate;
        //options.VideoEncoding = videoCoding;
        //options.Simulcast = true;
        //options.Source = TrackSource.SourceCamera;

        //var publish = room.LocalParticipant.PublishTrack(track, options);
        //yield return publish;

        //if (!publish.IsError)
        //{
        //    Debug.Log("Track published!");
        //}

        //source.Start();
        //StartCoroutine(source.Update());
    }
}