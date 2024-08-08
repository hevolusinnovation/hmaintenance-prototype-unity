using LiveKit;
using LiveKit.Proto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomAudioSender : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _retryOnError = default;

    [Header("References")]
    [SerializeField] private string _microphone = default;
    [SerializeField] private List<string> _microphoneDevices = default;
    [Space]
    [SerializeField] private AudioSource _audioSource = default;

    private RoomUser _user = default;

    protected override void Inizialize()
    {
        _microphoneDevices = Microphone.devices.ToList();
    }
    protected override void Dispose()
    {

    }

    private IEnumerator SendData()
    {

        var localSid = "my-audio-source";
        GameObject audObject = new GameObject(localSid);
        _audioSource.clip = Microphone.Start(_microphone, true, 2, (int)RtcAudioSource.DefaultSampleRate);
        _audioSource.loop = true;

        RtcAudioSource rtcSource = new RtcAudioSource(_audioSource);
        LocalAudioTrack track = LocalAudioTrack.CreateAudioTrack("my-audio-track", rtcSource, RoomSession.Room);

        var options = new TrackPublishOptions();
        options.AudioEncoding = new AudioEncoding();
        options.AudioEncoding.MaxBitrate = 64000;
        options.Source = TrackSource.SourceMicrophone;

        var publish = _user.GetLocalParticipant().PublishTrack(track, options);
        yield return publish;

        if (publish.IsError)
        {
            Debug.Log("Track is not published!");
            yield break;
        }

        rtcSource.Start();

    }

}