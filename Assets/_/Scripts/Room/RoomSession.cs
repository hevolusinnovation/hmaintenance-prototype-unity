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

public static class RoomSession
{
    public static Room Room { get; private set; }
    public static List<Participant> Participants { get; private set; }
    public static List<RoomBehaviour> RoomBehaviours { get; private set; }

    public static Action<SessionPhaseType> OnSessionPhaseChange = default;

    private static SessionPhaseType _currentSessionPhase = default;

    #region Session
    public static bool TryAskSessionChange(SessionPhaseType type)
    {
        if(_currentSessionPhase == type)
        {
            Debug.Log($"[RoomSession] ~ [TryCommunicateSession] - Session is already in phase {type}.");
            return false;
        }

        _currentSessionPhase = type;
        OnSessionPhaseChange?.Invoke(type);
        return true;
    }
    #endregion

    #region Room
    public static bool Initialize(Room room)
    {
        if (room == null)
            return false;

        Room = room;
        SubscribeToRoomEvents();
        OnSessionPhaseChange?.Invoke(SessionPhaseType.Start);
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
    #endregion

    #region Participant
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
    #endregion

    #region Behaviour
    public static void AddBehaviour(RoomBehaviour behaviour)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Remote Connected Behaviour Is Not Valid.");
            return;
        }

        if (RoomBehaviours.Contains(behaviour))
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Remote Connected Behaviour Is Already Present.");
            SyncSession(behaviour);
            return;
        }

        Debug.Log($"[RoomSession] ~ [AddBehaviour] - Remote Connected Behaviour Is Added.");
        RoomBehaviours.Add(behaviour);
        SyncSession(behaviour);
    }
    public static void RemoveBehaviour(RoomBehaviour behaviour)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour Is Not Valid.");
            return;
        }

        if (!RoomBehaviours.Contains(behaviour))
        {
            Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour Is Not Present.");
            return;
        }

        Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour Is Removed.");
        RoomBehaviours.Remove(behaviour);
    }
    private static void SyncSession(RoomBehaviour behaviour)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Behaviour Is Not Valid.");
            return;
        }

        behaviour.SessionPhase(_currentSessionPhase);
    }
    #endregion 

    public static Participant[] GetRemotePartecipantsAsArray() { return Participants.ToArray(); }
}