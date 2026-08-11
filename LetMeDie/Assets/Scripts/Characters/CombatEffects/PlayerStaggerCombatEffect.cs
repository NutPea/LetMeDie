using UnityEngine;

[CreateAssetMenu(menuName = "CombatEffect/Player/Stagger", fileName = "Stagger")]
public class PlayerStaggerCombatEffect : CombatEffect
{
    [SerializeField] private float minStaggerStrength = 0.0f;
    [SerializeField] private float maxStaggerStrength = 1.0f;
    [SerializeField] private float staggerTime = 0.5f;

    [SerializeField] private float minKnockBackStrength = 2;
    [SerializeField] private float maxKnockBackStrength = 5;
    [SerializeField] private float knockBackLength = 0.5f;

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
        knockBackPercentageStrength = 1;
    }

    public override void ResolveCombatEffect(Transform victim)
    {
        base.ResolveCombatEffect(victim);
        if(victim.TryGetComponent(out BaseEnemyCombat enemyCombat))
        {
            EnemyData enemyData = enemyCombat.BaseEnemyController.EnemyData;
            if (CalculateStagger(Mathf.Lerp(minStaggerStrength, maxStaggerStrength, knockBackPercentageStrength), enemyData.Mass))
            {
                enemyCombat.Stagger(staggerTime);
                Vector3 knockBackDir = victim.transform.position - Offender.transform.position;
                knockBackDir.y = 0;
                knockBackDir = knockBackDir.normalized;

                enemyCombat.BaseEnemyMovement
                    .Knockback(knockBackDir * CalculateKnockBackStregth(Mathf.Lerp(minKnockBackStrength, maxKnockBackStrength, knockBackPercentageStrength), enemyData.Mass), knockBackLength);
            }


        }
    }


}
