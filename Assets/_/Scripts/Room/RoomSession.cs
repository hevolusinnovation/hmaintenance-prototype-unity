using LiveKit;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[Flags]
public enum SessionPhaseType
{
    None = 0,
    Home = 1,
    ConnectionStarted = 2,
    ConnectionCompleted = 4,
    ConnectionError = 8,
    ConnectionRetry = 16,
    Start = 32,
    End = 64,
    Error = 128
}

public static class RoomSession
{
    public static Room Room { get; private set; }
    public static List<Participant> Participants { get; private set; } = new List<Participant>();
    public static List<RoomBehaviour> RoomBehaviours { get; private set; } = new List<RoomBehaviour>();

    public static Action<SessionPhaseType> OnSessionPhaseChange = default;

    private static SessionPhaseType _currentSessionPhase = default;

    #region Session
    public static bool TryAskSessionChange(SessionPhaseType type)
    {
        if (_currentSessionPhase == type)
        {
            Debug.Log($"[RoomSession] ~ [TryAskSessionChange] - Session Is Already In Phase {type}.");
            return false;
        }

        Debug.Log($"[RoomSession] ~ [TryAskSessionChange] - Moving To Session Phase {type}.");
        _currentSessionPhase = type;
        OnSessionPhaseChange?.Invoke(type);
        SyncSession();
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
        TryAskSessionChange(SessionPhaseType.Start);
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
    public static void AddBehaviour(RoomBehaviour behaviour, bool update = false)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Connected Behaviour {behaviour.GetType().Name} Is Not Valid.");
            return;
        }

        if (RoomBehaviours.Contains(behaviour))
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Connected Behaviour {behaviour.GetType().Name} Is Already Present.");

            if (update)
            {
                SyncSession(behaviour);
            }
            return;
        }

        Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Connected Behaviour {behaviour.GetType().Name} Is Added.");
        RoomBehaviours.Add(behaviour);
    }
    public static void RemoveBehaviour(RoomBehaviour behaviour)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour {behaviour.GetType().Name} Is Not Valid.");
            return;
        }

        if (!RoomBehaviours.Contains(behaviour))
        {
            Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour {behaviour.GetType().Name} Is Not Present.");
            return;
        }

        Debug.Log($"[RoomSession] ~ [RemoveBehaviour] - Remote Connected Behaviour {behaviour.GetType().Name} Is Removed.");
        RoomBehaviours.Remove(behaviour);
    }
    public static void SyncSession()
    {
        if (RoomBehaviours.Count <= 0)
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Behaviours To Sync Not Registered.");
            return;
        }

        RoomBehaviours.ForEach(behaviour => SyncSession(behaviour));
    }
    public static void SyncSession(RoomBehaviour behaviour)
    {
        if (behaviour == null)
        {
            Debug.Log($"[RoomSession] ~ [AddBehaviour] - Room Behaviour {behaviour.GetType().Name} Is Not Valid.");
            return;
        }

        behaviour.SessionPhase(_currentSessionPhase);
    }
    #endregion 

    public static Participant[] GetRemotePartecipantsAsArray() { return Participants.ToArray(); }
}