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
    [Header("Behaviour Settings")]
    [SerializeField] protected RoomBehaviourSettings _settings = default;

    protected virtual void Awake()
    {
        RoomSession.AddBehaviour(this);
    }
    protected virtual void OnDestroy()
    {
        RoomSession.RemoveBehaviour(this);
    }

    public void SessionPhase(SessionPhaseType phase)
    {
        Debug.Log($"[RoomBehaviour] - [SessionPhase] ~ {GetType().Name} Behaviour Session Phase Sync: {phase}.");

        if (_settings.HasInizialization(phase))
        {
            Debug.Log($"[RoomBehaviour] - [SessionPhase] ~ {GetType().Name} Initialization.");
            Inizialize();
        }

        if (_settings.HasDisposal(phase))
        {
            Debug.Log($"[RoomBehaviour] - [SessionPhase] ~ {GetType().Name} Disposal.");
            Dispose();
        }
    }

    protected abstract void Inizialize();
    protected abstract void Dispose();
}