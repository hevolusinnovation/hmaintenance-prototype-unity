using LiveKit;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[Flags]
public enum SessionPhaseType
{
    None = 0,
    Connection = 1,
    Start = 2,
    End = 4
}

[Serializable]
public struct RoomSettings
{
    [field:SerializeField] public string Name { get; set; }
}

public static class RoomSession
{
    public static RoomSettings RoomSettings {  get; private set; }
    public static Room Room { get; private set; }
    public static List<Participant> Participants { get; private set; }

    public static Action<SessionPhaseType> OnSessionPhaseChange = default;

    public static bool Initialize(Room room)
    {
        if (room == null)
            return false;

        Room = room;
        SubscribeToRoomEvents();
        return true;
    }
    public static bool Dispose()
    {
        UnsubscribeToRoomEvents();
        return true;
    }

    private static void SubscribeToRoomEvents()
    {
        if (Room == null)
        {
            Debug.Log($"[RoomSession] ~ [SubscribeToRoomEvents] - Room Tracked Is Not Valid.");
            return;
        }

        Room.ParticipantConnected += AddPartecipant;
        Room.ParticipantDisconnected += RemovePartecipant;
    }
    private static void UnsubscribeToRoomEvents()
    {
        if (Room == null)
        {
            Debug.Log($"[RoomSession] ~ [UnsubscribeToRoomEvents] - Room Tracked Is Not Valid.");
            return;
        }

        Room.ParticipantConnected -= AddPartecipant;
        Room.ParticipantDisconnected -= RemovePartecipant;
    }

    private static void AddPartecipant(Participant participant)
    {
        if (participant == null)
        {
            Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Not Valid.");
            return;
        }

        if (Participants.Contains(participant))
        {
            Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Already Present.");
            return;
        }

        Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Added.");
        Participants.Add(participant);
    }
    private static void RemovePartecipant(Participant participant)
    {
        if (participant == null)
        {
            Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Not Valid.");
            return;
        }

        if (!Participants.Contains(participant))
        {
            Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Not Present.");
            return;
        }

        Debug.Log($"[RoomSession] ~ [AddPartecipant] - Remote Connected Partecipant Is Removed.");
        Participants.Remove(participant);
    }

    public static Participant[] GetRemotePartecipantsAsArray() { return Participants.ToArray(); }
}