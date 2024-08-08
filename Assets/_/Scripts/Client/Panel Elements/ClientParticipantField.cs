using UnityEngine;

public class ClientParticipantField : ClientPanel
{
    [Header("Settings")]
    [SerializeField] private bool _isLocal = default;

    [Header("References")]
    [SerializeField] private RoomUser _roomUser = default;
    [Space]
    [SerializeField] private RoomDataSender _roomDataSender = default;
    [SerializeField] private RoomDataReceiver _roomDataReceiver = default;
    [Space]
    [SerializeField] private RoomVideoSender _roomVideoSender = default;
    [SerializeField] private RoomVideoReceiver _roomVideoReceiver = default;
    [Space]
    [SerializeField] private RoomAudioSender _roomAudioSender = default;
    [SerializeField] private RoomAudioReceiver _roomAudioReceiver = default;


}