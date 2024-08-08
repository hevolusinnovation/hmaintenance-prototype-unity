using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientAccessPanel : ClientPanel
{
    [Header("References")]
    [SerializeField] private TMP_InputField _roomNameInputField = default;
    [SerializeField] private TMP_InputField _participantNameInputField = default;
    [SerializeField] private Button _joinRoomButton = default;

    private RoomUserAuthentication _userAuthenticationData = new RoomUserAuthentication(); 

    public Action<RoomUserAuthentication> OnTryJoinRoomRequest = default;

    private void Awake()
    {
        if(_participantNameInputField != null)
        {
            _participantNameInputField.onEndEdit.AddListener(name => _userAuthenticationData.ParticipantName = name);
        }
        if (_roomNameInputField != null)
        {
            _roomNameInputField.onEndEdit.AddListener(name => _userAuthenticationData.RoomName = name);
        }
        if(_joinRoomButton != null)
        {
            _joinRoomButton.onClick.AddListener(TryJoinRoom);
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

    private void TryJoinRoom()
    {
        OnTryJoinRoomRequest?.Invoke(_userAuthenticationData);
    }
}
