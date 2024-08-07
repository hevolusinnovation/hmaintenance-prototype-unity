using LiveKit;
using UnityEngine;

public class RoomUser : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _name = default;
    [Space]
    [SerializeField] private bool _isCameraEnabled = default;
    [SerializeField] private bool _isMicrophoneEnabled = default;
    [SerializeField] private bool _isScreenShareEnabled = default;

    private LocalParticipant _localParticipant = default;

    protected override void Inizialize()
    {
        _localParticipant = RoomSession.Room.LocalParticipant;
    }
    protected override void Dispose()
    {

    }

    public string GetParticipantName() { return _name; }
    public LocalParticipant GetLocalParticipant() { return _localParticipant; }
}