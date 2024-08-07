using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientAccessPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField _roomNameInputField = default;
    [SerializeField] private TMP_InputField _participantNameInputField = default;
    [SerializeField] private Button _joinRoomButton = default;
    [Space]
    [SerializeField] private RoomSettings _roomSettings = default;
    [SerializeField] private RoomUser _roomUser = default;

    public Action<RoomUserAuthentication> OnJoinRoomAuthentication = default;

    private void Awake()
    {
        if(_participantNameInputField != null)
        {
            _participantNameInputField.onEndEdit.AddListener(name => _roomUser.SetParticipantName(name));
        }
        if (_roomNameInputField != null)
        {
            _roomNameInputField.onEndEdit.AddListener(name => _roomSettings.SetRoomName(name));
        }
        if(_joinRoomButton != null)
        {
            _joinRoomButton.onClick.AddListener(GenerateRoomUserModel);
        }
    }
    private void OnDestroy()
    {
        if (_participantNameInputField != null)
        {
            _participantNameInputField.onEndEdit.RemoveAllListeners();
        }
        if (_roomNameInputField != null)
        {
            _roomNameInputField.onEndEdit.RemoveAllListeners();
        }
        if (_joinRoomButton != null)
        {
            _joinRoomButton.onClick.RemoveAllListeners();
        }
    }

    private void GenerateRoomUserModel()
    {
        RoomUserAuthentication roomUserModel = new RoomUserAuthentication();
        roomUserModel.RoomName = _roomSettings.GetRoomName();
        roomUserModel.ParticipantName = _roomUser.GetParticipantName();

        OnJoinRoomAuthentication?.Invoke(roomUserModel);
    }
}
