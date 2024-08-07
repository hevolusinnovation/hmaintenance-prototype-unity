using System;
using UnityEngine;

[Serializable]
public struct RoomBehaviourSettings
{
    [SerializeField] private SessionPhaseType _initializationPhase;
    [SerializeField] private SessionPhaseType _disposalPhase;

    public bool HasInizialization(SessionPhaseType behaviourInizialization) => _initializationPhase.HasFlag(behaviourInizialization);
    public bool HasDisposal(SessionPhaseType behaviourInizialization) => _disposalPhase.HasFlag(behaviourInizialization);
}

public abstract class RoomBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected RoomBehaviourSettings _behaviourSettings = default;

    protected virtual void Awake()
    {
        RoomSession.OnSessionPhaseChange += SessionPhase;
    }
    protected virtual void OnDestroy()
    {
        RoomSession.OnSessionPhaseChange -= SessionPhase;
    }

    protected void SessionPhase(SessionPhaseType phase)
    {
        if (_behaviourSettings.HasInizialization(phase))
            Inizialize();

        if (_behaviourSettings.HasDisposal(phase))
            Dispose();
    }

    protected abstract void Inizialize();
    protected abstract void Dispose();
}