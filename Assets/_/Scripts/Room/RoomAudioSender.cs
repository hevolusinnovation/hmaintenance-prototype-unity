using LiveKit;
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
    [Space]
    [SerializeField] private List<string> _microphoneDevices = default;
    [Space]
    [SerializeField] private RoomUser _user = default;

    //private Queue<LocalTrack> _encodedTracks = new Queue<LocalTrack>();

    protected override void Inizialize()
    {
        _microphoneDevices = Microphone.devices.ToList();
    }
    protected override void Dispose()
    {
    }

    //private void Update()
    //{
    //    if (_user.GetLocalParticipant() != null && _user.GetLocalParticipant().IsSpeaking)
    //    {
    //        _user.GetLocalParticipant().CreateTracks();
    //    }
    //}

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
    //        Debug.Log($"[RoomAudioSender] - [CO_SendData] ~ Publishing AudioData Operation Ends With Error.");

    //        if (_retryOnError)
    //        {
    //            yield return CO_SendData(localTrack);

    //            Debug.Log($"[RoomAudioSender] - [CO_SendData] ~ Retry Attemp To Publishing AudioData Operation Ends Correctly.");
    //            yield break;
    //        }

    //        yield break;
    //    }

    //    if (promise.IsDone)
    //    {
    //        Debug.Log($"[RoomAudioSender] - [CO_SendData] ~ Publishing AudioData Operation Ends Correctly.");
    //    }
    //}
}