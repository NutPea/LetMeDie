using System;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyHandler : MonoBehaviour
{
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float attackStopDistance = 2.2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int knockBackStregth = 0;

    [SerializeField] private int damage = 2;
    [SerializeField] private LayerMask attackMask;

    [SerializeField] private Animator animator;
    private Rigidbody rb;
    private HealthManager healthManager;
    private Transform player;
    private NavMeshAgent agent;

    private bool isAggro;
    private bool isAttacking;

    [SerializeField] private float attackMoveAmount = 1f;
    private bool lookAtPlayer;

    [SerializeField] private float lookAtSpeed = 1f;
    private bool isDead;


    void Start()
    {
        player = PlayerSingelton.instance.transform;
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(StopEverything);
        rb = GetComponent<Rigidbody>();
        agent= GetComponent<NavMeshAgent>();
        isAggro = false;
    }

    private void StopEverything(GameObject diedObject)
    {
        agent.enabled = false;
        isDead = true;
        animator.SetBool("Attack", false);
        CancelInvoke(nameof(ResetAnim));
        CancelInvoke(nameof(ResetAttack));
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }   
        if (lookAtPlayer)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                lookAtSpeed * Time.deltaTime
            );
        }

        float playerDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (playerDistance < aggroRange)
        {
            isAggro = true;
        }
        animator.SetFloat("Movement", agent.velocity.magnitude);
        if (!isAggro || isAttacking)
        {
            return;
        }
        if(playerDistance < attackStopDistance)
        {
            Attack();
           
        }
        if (!isAttacking)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackDistance, attackRange);
    }

    private void Attack()
    {
        if (isAttacking)
        {
            return;
        }
        animator.SetBool("Attack", true);
        isAttacking = true;
        agent.isStopped = true;
        agent.enabled = false;
        Invoke(nameof(ResetAnim), attackCooldown/2);
        lookAtPlayer = true;
    }

    private void ResetAnim()
    {
        animator.SetBool("Attack", false);
    }
    private void ResetAttack()
    {
        isAttacking = false;
        agent.enabled = true;
        agent.isStopped = false;
    }

    public void AttackTrigger()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position + transform.forward * attackDistance, attackRange, attackMask);
        if(collider.Length > 0)
        {
            foreach (Collider col in collider) {
                HealthManager healthManager = col.GetComponent<HealthManager>();
                healthManager.InflictDamage(damage, knockBackStregth, TeamFlag.Enemy, transform);
            }
        }

        rb.AddForce(transform.forward * attackMoveAmount,ForceMode.Impulse);
        Invoke(nameof(ResetAttack), attackCooldown);
        //Look at player
        //MoveForward
        //ResetAttack
    }
}
