using LiveKit;
using System;
using System.Collections;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _timeout = default;

    [Header("References")]
    [SerializeField] private TokenRequester _tokenRequester = default;

    private Room _room = default;
    private UserToken _userToken = default;

    private WaitForSeconds _waiter = default;

    public Action<Room> OnRoomConnected = default;

    private void Awake()
    {
        if(_tokenRequester != null)
        {
            _tokenRequester.OnUserTokenReceived += ApplyTokenToConnect;
        }
    }
    private void OnDestroy()
    {
        if (_tokenRequester != null)
        {
            _tokenRequester.OnUserTokenReceived -= ApplyTokenToConnect;
        }
    }
    private void Start()
    {
        _waiter = new WaitForSeconds(_timeout);
    }

    private void ApplyTokenToConnect(UserToken token)
    {
        if(token == null)
        {
            return;
        }

        _userToken = token;
        ConnectRoom();
    }

    [ContextMenu(nameof(CO_ConnectRoomOperation))]
    private void ConnectRoom()
    {
        StartCoroutine(CO_ConnectRoomOperation());
    }

    private IEnumerator CO_ConnectRoomOperation()
    {
        Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - Starting Connect Room Operation.");

        RoomSession.OnSessionPhaseChange?.Invoke(SessionPhaseType.Connection);

        _room = new Room();
        ConnectInstruction connectOperation = _room.Connect(APIEndpoint.LOCAL_URL, _userToken.Token, new RoomOptions());
        yield return connectOperation;

        if (!connectOperation.IsError)
        {
            Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - No Errors Encountered Connecting To Room.");
        }
        else
        {
            Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - Connecting To Room: {connectOperation.IsDone}.");
        }

        if (connectOperation.IsDone)
        {
            Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - Connect Room Operation Is Complete.");
            RoomSession.Initialize(_room);
            OnRoomConnected?.Invoke(_room);
            yield break;
        }

        Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - Connect Room Operation Failed: {connectOperation}.");
        Debug.Log($"[RoomConnector] ~ [ConnectRoomOperation] - Retrying Connect Room Operation In {_timeout} Seconds.");
        yield return _waiter;

        yield return CO_ConnectRoomOperation();
    }
}