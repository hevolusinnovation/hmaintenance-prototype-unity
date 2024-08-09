using LiveKit;
using TMPro;
using UnityEngine;

public class RoomUser : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _name = default;
    [Space]
    [SerializeField] private bool _isCameraEnabled = default;
    [SerializeField] private bool _isMicrophoneEnabled = default;
    [SerializeField] private bool _isScreenShareEnabled = default;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _userNameText = default;

    private Participant _participant = default;
    private LocalParticipant _localParticipant = default;
    private RemoteParticipant _remoteParticipant = default;

    protected override void Inizialize()
    {
        //_userNameText.text = _participant.Name;
    }
    protected override void Dispose()
    {

    }

    public void AssignParticipant(Participant participant)
    {
        _participant = participant;
    }
    public void SetParticipantName(string name)
    {
        _name = name;
        _userNameText.text = name;
    }
    public string GetParticipantName() { return _name; }
    public Participant GetParticipant() { return _participant; }
    public bool TryGetLocalParticipant(out LocalParticipant participant)
    {
        if(_localParticipant != null)
        {
            participant = _localParticipant;
            return true;
        }

        bool isLocalParticipant = RoomSession.Room.LocalParticipant == _participant;
        participant = default;

        if (isLocalParticipant)
        {
            _localParticipant = _participant as LocalParticipant;
            participant = _localParticipant;
        }

        return isLocalParticipant;
    }
    public bool TryGetRemoteParticipant(out RemoteParticipant participant)
    {
        if (_remoteParticipant != null)
        {
            participant = _remoteParticipant;
            return true;
        }

        bool isRemoteParticipant = RoomSession.Room.LocalParticipant != _participant;
        participant = default;

        if (isRemoteParticipant)
        {
            _remoteParticipant = _participant as RemoteParticipant;
            participant = _remoteParticipant;
        }

        return isRemoteParticipant;
    }
}