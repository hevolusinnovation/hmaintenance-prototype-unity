using System.Collections.Generic;
using UnityEngine;

public class ClientPanelController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SessionPhaseType _startingSessionPhase = SessionPhaseType.None;

    [Header("References")]
    [SerializeField] private List<ClientPanel> _clientPanels = new List<ClientPanel>();

    private void Awake()
    {
        RoomSession.OnSessionPhaseChange += HandlePanels;
    }
    private void Start()
    {
        HandlePanels(_startingSessionPhase);
    }
    private void OnDestroy()
    {
        RoomSession.OnSessionPhaseChange -= HandlePanels;
    }

    private void HandlePanels(SessionPhaseType sessionPhase)
    {
        if (_clientPanels.Count <= 0)
            return;

        _clientPanels.ForEach(panel => panel.Enable(sessionPhase));
    }
}