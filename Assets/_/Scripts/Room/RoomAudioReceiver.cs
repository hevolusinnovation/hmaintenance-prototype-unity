using LiveKit;
using UnityEngine;

public class RoomAudioReceiver : RoomBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource _audioSource = default;

    protected override void Inizialize()
    {
    //    RoomSession.Room.TrackSubscribed += ReceiveAudioData;
    }
    protected override void Dispose()
    {
    //    RoomSession.Room.TrackSubscribed -= ReceiveAudioData;
    }

    //private void ReceiveAudioData(RemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    //{
    //    if (track.Kind == TrackKind.Audio)
    //    {
    //        HTMLAudioElement audio = track.Attach() as HTMLAudioElement;
    //    }
    //}
}