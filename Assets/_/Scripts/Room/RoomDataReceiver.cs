using LiveKit;
using UnityEngine;

public class RoomDataReceiver : RoomBehaviour
{
    protected override void Inizialize()
    {
        //RoomSession.Room.DataReceived += ReceiveData;
    }
    protected override void Dispose()
    {
        //RoomSession.Room.DataReceived -= ReceiveData;
    }

    //private void ReceiveData(byte[] data, RemoteParticipant participant, DataPacketKind? kind)
    //{
    //    Debug.Log($"Received data of type {kind.Value} from {participant.Name}.");
    //}
}