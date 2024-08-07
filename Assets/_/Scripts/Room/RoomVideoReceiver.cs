using LiveKit;
using UnityEngine;
using UnityEngine.UI;

public class RoomVideoReceiver : RoomBehaviour
{
    [Header("References")]
    [SerializeField] private RawImage _rawImage = default;

    protected override void Inizialize()
    {
        //RoomSession.Room.TrackSubscribed += ReceiveVideoData;
    }
    protected override void Dispose()
    {
        //RoomSession.Room.TrackSubscribed -= ReceiveVideoData;
    }

    //private void ReceiveVideoData(RemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    //{
    //    if (track.Kind == TrackKind.Video)
    //    {
    //        HTMLVideoElement video = track.Attach() as HTMLVideoElement;
    //        video.VideoReceived += tex =>
    //        {
    //            _rawImage.texture = tex;
    //        };
    //    }
    //}
}