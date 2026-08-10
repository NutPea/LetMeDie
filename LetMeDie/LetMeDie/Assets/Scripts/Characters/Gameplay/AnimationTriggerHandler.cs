using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerHandler : MonoBehaviour
{
    public UnityEvent OnAttackTrigger = new();

    public void AttackTrigger()
    {
        OnAttackTrigger.Invoke();
    }
}
