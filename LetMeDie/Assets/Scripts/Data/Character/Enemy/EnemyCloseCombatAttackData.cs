using UnityEngine;


[CreateAssetMenu(fileName = "EnemyCloseCombatAttackData", menuName = "Character/EnemyAttacks/EnemyCloseCombatAttackData", order = 1)]
public class EnemyCloseCombatAttackData : EnemyAttackData
{
    [Header("CLOSE COMBAT")]
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private int damage;
    [SerializeField] private float attackCollisionSphereRadius = 1f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackMoveAmount = 1f;
    [SerializeField] private float lookAtPlayerSpeed = 180f;

    private Rigidbody rb;
    private bool hasAttacked;
    private bool lookAtPlayer;

    public override void Init(BaseEnemyController enemy, EnemyData enemyData)
    {
        base.Init(enemy, enemyData);
        rb = enemy.GetComponent<Rigidbody>();
    }


    public override void Select(BaseEnemyCombat baseEnemyCombat, Transform Player)
    {
        base.Select(baseEnemyCombat, Player);
        hasAttacked = false;
        lookAtPlayer = false;
    }

    public override void AttackUpdate(BaseEnemyCombat baseEnemyCombat, Transform Player)
    {
        base.AttackUpdate(baseEnemyCombat, Player);

        if (lookAtPlayer){
            LookAtPlayer(baseEnemyCombat.transform, Player.transform, lookAtPlayerSpeed);
        }

        if (hasAttacked)
        {
            return;
        }


        float playerDistance = Vector3.Distance(Player.transform.position, baseEnemyCombat.transform.position);
        if (playerDistance < currentEnemyData.AttackStopDistance)
        {

            baseEnemyCombat.BaseEnemyMovement.StopMovement();
            PlayAttackAnimation();
            hasAttacked = true;
            lookAtPlayer = true;
        }
    }

    public override void AntizipationAttack(BaseEnemyCombat baseEnemyCombat)
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(baseEnemyCombat.transform.forward * attackMoveAmount, ForceMode.Impulse);
    }

    public override void Attack(BaseEnemyCombat baseEnemyCombat)
    {
        Collider[] collider = Physics.OverlapSphere(baseEnemyCombat.transform.position + attackOffset + baseEnemyCombat.transform.forward * attackDistance, attackCollisionSphereRadius, currentEnemyData.AttackMask);
        if (collider.Length > 0)
        {
            foreach (Collider col in collider)
            {
                HealthManager healthManager = col.GetComponent<HealthManager>();
                healthManager.InflictDamage(damage, TeamFlag.Enemy, baseEnemyCombat.transform);
            }
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (ShowDebug){
            ShowHitDebug(baseEnemyCombat.transform.position + attackOffset + baseEnemyCombat.transform.forward * attackDistance, attackCollisionSphereRadius);
        }
    }

    private void ShowHitDebug(Vector3 hitPosition,float hitSize)
    {
        GameObject sphere = Instantiate(SGameManager.Instance.Sphere);
        sphere.transform.position = hitPosition;
        sphere.transform.localScale = new Vector3(hitSize, hitSize, hitSize);
        Destroy(sphere, 0.2f);
    }

    public override void AttackFinished(BaseEnemyCombat baseEnemyCombat)
    {
        baseEnemyCombat.BaseEnemyMovement.StartMovement();
        lookAtPlayer = false;
    }


}
