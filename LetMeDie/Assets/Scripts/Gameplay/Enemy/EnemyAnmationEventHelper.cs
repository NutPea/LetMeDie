using UnityEngine;
using UnityEngine.Events;

public class EnemyAnmationEventHelper : MonoBehaviour
{
    public UnityEvent OnAntizipationAttack = new UnityEvent();

    public UnityEvent OnAnimationAttack = new UnityEvent();

    public void AttackEvent()
    {
        OnAnimationAttack.Invoke();
    }

    public void AntizipationAttack()
    {
        OnAntizipationAttack.Invoke();
    }

}
