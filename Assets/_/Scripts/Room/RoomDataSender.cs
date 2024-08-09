using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RoomDataSender : RoomBehaviour
{
    private RoomUser _user = default;

    protected override void Inizialize()
    {

    }
    protected override void Dispose()
    {

    }

    [ContextMenu(nameof(PackData))]
    private void PackData()
    {

        Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send.");
        object obj = new object();

        if (obj == null)
        {
            Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send Is Null.");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            byte[] result = ms.ToArray();
            Debug.Log($"[RoomDataSender] - [PackData] ~ Success: Encoded Pack Data.");
        }

    }

    public void SendData(byte[] data)
    {
        if (!_user.TryGetLocalParticipant(out var participant))
        {
            Debug.Log($"[RoomDataSender] - [SendData] ~ Local Participant Was Not Found.");
            return;
        }

        participant.PublishData(data);
    }

}