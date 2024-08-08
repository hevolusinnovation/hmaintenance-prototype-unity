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
        if (_settings.HasInizialization(phase))
            Inizialize();

        if (_settings.HasDisposal(phase))
            Dispose();
    }

    protected abstract void Inizialize();
    protected abstract void Dispose();
}