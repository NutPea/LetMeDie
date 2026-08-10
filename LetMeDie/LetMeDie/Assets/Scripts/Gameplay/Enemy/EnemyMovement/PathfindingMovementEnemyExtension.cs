using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class PathfindingMovementEnemyExtension : BaseEnemyMovement
{
    private NavMeshAgent agent;
    private bool lookOnPlayer;
    private bool isDead;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = BaseEnemyController.EnemyData.MovementSpeed;
        agent.acceleration = BaseEnemyController.EnemyData.AccelerationSpeed;
    }

    protected override void StopEverything(GameObject died)
    {
        base.StopEverything(died);
        isDead = true;
        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
    }
    protected override void OnAggro()
    {
        base.OnAggro();
        lookOnPlayer = true;
    }

    public override void StopMovement()
    {
        if (isDead){
            return;
        }
        agent.isStopped = true;
        agent.enabled = false;
        SetMovementAnimationValue(0f);
    }

    public override void StartMovement()
    {
        if (isDead){
            return;
        }
        agent.enabled = true;
        agent.isStopped = false;
    }

    public override void Knockback(Vector3 dir)
    {
        StopMovement();
        SimpleRigidbodyKnockback(dir);
    }

    public override void ResetKnockback()
    {
        StartMovement();
        base.ResetKnockback();

    }  

    private void Update()
    {
        if (!agent.enabled)
        {
            return;
        }


        if (!lookOnPlayer) {
            return;
        }
        SetMovementAnimationValue(agent.velocity.magnitude / agent.speed);
        agent.SetDestination(player.position);
    }

}
