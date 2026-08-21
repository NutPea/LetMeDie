using UnityEngine;
using UnityEngine.Timeline;


[CreateAssetMenu(fileName = "EnemySimpleDamageAreaSpawn", menuName = "Character/EnemyAttacks/EnemySimpleDamageAreaSpawn", order = 1)]
public class EnemySimpleDamageAreaSpawn : EnemyAttackData
{
    [SerializeField] private GameObject damageArePrefab;
    [SerializeField] private float areaSize = 2f;
    [SerializeField] private float timeUntilDamage = 1f;
    [SerializeField] private int damageAmount = 50;
    private Transform player;
    private Rigidbody rb;

    public override void Init(BaseEnemyController enemy, EnemyData enemyData)
    {
        base.Init(enemy, enemyData);
        rb = enemy.GetComponent<Rigidbody>();
    }

    public override void Select(BaseEnemyCombat baseEnemyCombat, Transform Player)
    {
        base.Select(baseEnemyCombat, Player);
        PlayAttackAnimation();
        player = Player;
        baseEnemyCombat.BaseEnemyMovement.StopMovement();
    }


    public override void Attack(BaseEnemyCombat baseEnemyCombat)
    {
        GameObject damageAreaGameobject = Instantiate(damageArePrefab);
        DamageArea damageArea = damageAreaGameobject.GetComponent<DamageArea>();
        damageArea.transform.position = player.transform.position;
        damageArea.StartDamageArea(areaSize, timeUntilDamage, damageAmount);
    }


    public override void AttackFinished(BaseEnemyCombat baseEnemyCombat)
    {
        RigidbodyConstraints beforeConstrains = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = beforeConstrains;
        baseEnemyCombat.BaseEnemyMovement.StartMovement();
    }

}
