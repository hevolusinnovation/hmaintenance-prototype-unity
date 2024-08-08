using UnityEngine;

public class ClientRoomSessionPanel : ClientPanel
{
    [Header("References")]
    [SerializeField] private RectTransform _participantGridContainer = default;

    [Header("Prefabs")]
    [SerializeField] private ClientParticipantField _participantFieldSource = default;

    private void Awake()
    {
        
    }
}