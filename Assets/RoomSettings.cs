using LiveKit;
using UnityEngine;

public class RoomSettings : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _name = default;
    [Space]
    [SerializeField] private bool _isCameraEnabled = default;
    [SerializeField] private bool _isMicrophoneEnabled = default;
    [SerializeField] private bool _isScreenShareEnabled = default;

    private Room _localRoom = default;

    protected override void Inizialize()
    {
        _localRoom = RoomSession.Room;
    }
    protected override void Dispose()
    {

    }

    public void SetRoomName(string name) { _name = name; }
    public string GetRoomName() { return _name; }
    public Room GetLocalParticipant() { return _localRoom; }

}
