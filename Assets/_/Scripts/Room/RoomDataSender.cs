using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RoomDataSender : RoomBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _retryOnError = default;

    [Header("References")]
    [SerializeField] private Object _object = default;
    [Space]
    [SerializeField] private RoomUser _user = default;

    private Queue<byte[]> _encodedPackData = new Queue<byte[]>();

    protected override void Inizialize()
    {

    }
    protected override void Dispose()
    {

    }

    //private void Update()
    //{
    //    // TEMP
    //    if(_encodedPackData.Count <= 0)
    //        return;

    //    SendData(_encodedPackData.Dequeue(), DataPacketKind.RELIABLE, _retryOnError, RoomSession.GetRemotePartecipantsAsArray());
    //}

    [ContextMenu(nameof(PackData))]
    private void PackData()
    {
        Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send.");

        if (_object == null)
        {
            Debug.Log($"[RoomDataSender] - [PackData] ~ Packing Data To Send Is Null.");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, _object);
            _encodedPackData.Enqueue(ms.ToArray());
            Debug.Log($"[RoomDataSender] - [PackData] ~ Success: Encoded Pack Data.");
        }
    }

    public void SendData(byte[] data, bool retryOnError = false)
    {
        //StartCoroutine(CO_SendData(data));
    }
    //private IEnumerator CO_SendData(byte[] data)
    //{
    //    Debug.Log($"[RoomDataSender] - [CO_SendData] ~ Starting Send Data Coroutine Process.");

    //    _user.GetLocalParticipant().PublishData(data);
    //    yield return promise;

    //    if (promise.IsError)
    //    {
    //        Debug.Log($"[RoomDataSender] - [CO_SendData] ~ Publishing Data Operation Ends With Error.");

    //        if (_retryOnError)
    //        {
    //            yield return CO_SendData(data, kind, participants);

    //            Debug.Log($"[RoomDataSender] - [CO_SendData] ~ Retry Attemp To Publishing Data Operation Ends Correctly.");
    //            yield break;
    //        }

    //        yield break;
    //    }

    //    if (promise.IsDone)
    //    {
    //        Debug.Log($"[RoomDataSender] - [CO_SendData] ~ Publishing Data Operation Ends Correctly.");
    //    }
//}
}
