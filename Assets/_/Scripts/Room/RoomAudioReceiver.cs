using LiveKit;
using UnityEngine;

public class RoomAudioReceiver : RoomBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource _audioSource = default;

    protected override void Inizialize()
    {
        RoomSession.Room.TrackSubscribed += TrackSubscribed;
    }
    protected override void Dispose()
    {
        RoomSession.Room.TrackSubscribed -= TrackSubscribed;
    }

    void TrackSubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {

        if (track is RemoteAudioTrack audioTrack)
        {
            Debug.Log($"[RoomAudioReceiver] - [TrackSubscribed] - ");

            GameObject audObject = new GameObject(audioTrack.Sid);
            AudioStream stream = new AudioStream(audioTrack, _audioSource);
        }


    }

}