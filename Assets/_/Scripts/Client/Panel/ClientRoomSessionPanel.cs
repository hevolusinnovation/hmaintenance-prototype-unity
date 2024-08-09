using LiveKit;
using UnityEngine;

public class ClientRoomSessionPanel : ClientPanel
{
    [Header("References")]
    [SerializeField] private RectTransform _participantGridContainer = default;

    [Header("Prefabs")]
    [SerializeField] private ClientParticipantField _participantFieldSource = default;

    private void Awake()
    {
        RoomSession.OnSessionPhaseChange += DisposeParticipants;
    }
    private void OnDestroy()
    {
        RoomSession.OnSessionPhaseChange -= DisposeParticipants;
    }

    private void DisposeParticipants(SessionPhaseType phase)
    {
        Room currentRoom = RoomSession.Room;
        
        if(currentRoom == null)
            return;


        ClientParticipantField localParticipant = Instantiate(_participantFieldSource, _participantGridContainer);
        localParticipant.SetupRemoteParticipantInfo(RoomSession.Room.LocalParticipant);

        foreach (RemoteParticipant remoteParticipant in RoomSession.Room.RemoteParticipants.Values)
        {
            ClientParticipantField participant = Instantiate(_participantFieldSource, _participantGridContainer);
            participant.SetupRemoteParticipantInfo(remoteParticipant);
        }
    }
}