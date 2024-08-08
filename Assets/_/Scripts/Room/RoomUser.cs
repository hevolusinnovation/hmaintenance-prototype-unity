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

    private LocalParticipant _localParticipant = default;

    protected override void Inizialize()
    {

        _localParticipant = RoomSession.Room.LocalParticipant;
        _userNameText.text = _localParticipant.Name;

    }
    protected override void Dispose()
    {

    }

    public void SetParticipantName(string name) { _name = name; }   
    public string GetParticipantName() { return _name; }
    public LocalParticipant GetLocalParticipant() { return _localParticipant; }
}