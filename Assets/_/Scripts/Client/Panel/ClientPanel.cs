using UnityEngine;

public abstract class ClientPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] protected SessionPhaseType _enablingPhase = default;

    public virtual void Enable(SessionPhaseType currentPhase)
    {
        bool isEnabled = _enablingPhase.HasFlag(currentPhase);
        gameObject.SetActive(isEnabled);
    }
}
