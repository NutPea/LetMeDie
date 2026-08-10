using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Character/EnemyData", order = 1)]
public class EnemyData : HealthData
{
    [Header("Movement")]

    [SerializeField] private float movementSpeed = 5f;
    public float MovementSpeed => movementSpeed;


    [SerializeField] private float accelerationSpeed = 1f;
    public float AccelerationSpeed => accelerationSpeed;

    [Header("Combat")]
    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private float attackStopDistance = 2.2f;
    public float AttackStopDistance => attackStopDistance;

    [SerializeField] private float attackDistance = 1f;
    public float AttackDistance => attackDistance;

    [SerializeField] private float attackCooldown = 1f;
    public float AttackCooldown => attackCooldown;

    [SerializeField] private LayerMask attackMask;
    public LayerMask AttackMask => attackMask;
    [Header("Stagger")]

    [SerializeField] private float staggerPercentage = 1f;
    public float StaggerPercentage => staggerPercentage;

    [SerializeField] private int mass = 1;

    [SerializeField] private float staggerTime = 1f;
    public float StaggerTime => staggerTime;

    [SerializeField] private float knockbackPossibility = 1f;
    [SerializeField] private float knockBackResetTime = 0.3f;
    public float KnockBackResetTime => knockBackResetTime;

    private const int MAX_KNOCKBACK = 10;


    [SerializeField] private List<EnemyAttackData> initialEnemyAttackDatas = new();
    private List<EnemyAttackData> enemyAttackDatas = new();

    public bool HasAttacks => enemyAttackDatas.Count > 0;

    
    public void Init(BaseEnemyController enemy)
    {
        enemyAttackDatas = new();
        foreach(EnemyAttackData data in initialEnemyAttackDatas)
        {
            EnemyAttackData copyData = Instantiate(data);
            copyData.Init(enemy,this);
            enemyAttackDatas.Add(copyData);
        }
    }
    
    public EnemyAttackData GetAttack()
    {
        return enemyAttackDatas[0];
    }

    public virtual bool CalculateKnockbackPossibility(float knockBackStregth)
    {
        float knockBackPossibility = knockBackStregth / (float)mass*3;
        Debug.Log(knockBackPossibility);
        return Random.Range(0.0f, 1.0f) < knockBackPossibility;
    }

    public virtual float CalculateKnockbackStrength(float knockBackStregth)
    {
        float calculatedKnockbackStregth = knockBackStregth - mass;
        if(calculatedKnockbackStregth < 0)
        {
            calculatedKnockbackStregth = 0;
        }
        if(calculatedKnockbackStregth > MAX_KNOCKBACK)
        {
            calculatedKnockbackStregth = MAX_KNOCKBACK;
        }
        Debug.Log(calculatedKnockbackStregth);
        return calculatedKnockbackStregth;
    }

}
