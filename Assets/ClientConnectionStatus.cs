using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public enum ConnectionState
{
    Connecting = 0,
    Connected = 1,
    Error = 2,
    RetryConnection = 3,
    Disconnecting = 4,
    Disconnected = 5
}

[System.Serializable]
public struct ConnectionLiveStatus
{
    [field:SerializeField] public ConnectionState State { get; set; }
    [field: SerializeField] public string MessageLiveStatus { get; set; }
    [field: SerializeField] public Color MessageColor { get; set; }

    public bool CanProvideLiveStatus(ConnectionState state)
    {
        return State.HasFlag(state);
    }
}

public class ClientConnectionStatus : ClientPanel
{
    [Header("Settings")]
    [SerializeField] private List<ConnectionLiveStatus> _settings = new List<ConnectionLiveStatus>();

    [Header("References")]
    [SerializeField] private ColorLerpBehaviour _colorLerp = default;
    [SerializeField] private TextMeshProUGUI _connectionStatusText = default;

    public override void Enable(SessionPhaseType currentPhase)
    {
        base.Enable(currentPhase);

        ConnectionState state = ConvertPhaseToState(currentPhase);
        ConnectionLiveStatus liveStatus = _settings.FirstOrDefault(setting => setting.CanProvideLiveStatus(state));

        _connectionStatusText.text = liveStatus.MessageLiveStatus;
        _colorLerp.ChangeColorSetting(false, liveStatus.MessageColor, new Color(liveStatus.MessageColor.r, liveStatus.MessageColor.g, liveStatus.MessageColor.b, 0));
    }

    private ConnectionState ConvertPhaseToState(SessionPhaseType phase)
    {
        switch (phase)
        {
            case SessionPhaseType.ConnectionStarted:
                return ConnectionState.Connecting;
            case SessionPhaseType.ConnectionError:
                return ConnectionState.Error;
            case SessionPhaseType.ConnectionRetry:
                return ConnectionState.RetryConnection;
            case SessionPhaseType.ConnectionCompleted:
                return ConnectionState.Connected;
            default:
                return ConnectionState.Disconnected;
        }
    }
}