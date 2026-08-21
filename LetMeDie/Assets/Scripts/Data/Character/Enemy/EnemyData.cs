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

    [Header("Combat")]
    [SerializeField] private int damage;
    public int Damage => damage;


    [SerializeField] private float attackDistance = 1f;
    public float AttackDistance => attackDistance;

    [SerializeField] private float attackCooldown = 1f;
    public float AttackCooldown => attackCooldown;

    [SerializeField] private LayerMask attackMask;
    public LayerMask AttackMask => attackMask;

    [SerializeField] private float mass = 1;
    public float Mass => mass;  



    [SerializeField] private List<EnemyAttackData> initialEnemyAttackDatas = new();
    private List<EnemyAttackData> enemyAttackDatas = new();
    public List<EnemyAttackData> EnemyAttackDatas => enemyAttackDatas;


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

        for (int i = 0; i < 100; i++) {
            EnemyAttackData data = EnemyAttackDatas[Random.Range(0, EnemyAttackDatas.Count)];
            
            if (data.HasCooldown)
            {
                continue;
            }
            else
            {
                return data;
            }

        }

        return EnemyAttackDatas[0];
    }



}
