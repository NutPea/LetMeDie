using System;
using UnityEngine;




[CreateAssetMenu(menuName = "CombatEffect/Player/Knockback", fileName = "Knockback")]
public class PlayerKnockBackCombatEffect : CombatEffect
{
    [SerializeField] private float minKnockBackStrength;
    [SerializeField] private float maxKnockBackStrength;
    [SerializeField] private float knockBackLength;

    private float knockBackPercentageStrength = 1;
    public float KnockBackPercentageStrength
    {
        set
        {
            knockBackPercentageStrength = value;
        }
    }

    public override void Init(Transform offender)
    {
        base.Init(offender);
    }

    public override void ResolveCombatEffect(Transform victim)
    {
        base.ResolveCombatEffect(victim);
        if(victim.TryGetComponent(out BaseEnemyMovement baseEnemyMovement))
        {
            EnemyData enemyData = baseEnemyMovement.BaseEnemyController.EnemyData;
            Vector3 knockBackDir = victim.transform.position - Offender.transform.position;
            knockBackDir.y = 0;
            knockBackDir = knockBackDir.normalized;

            baseEnemyMovement.Knockback(knockBackDir * CalculateKnockBackStregth(Mathf.Lerp( minKnockBackStrength, maxKnockBackStrength,knockBackPercentageStrength), enemyData.Mass), knockBackLength);
        }
    }

}
