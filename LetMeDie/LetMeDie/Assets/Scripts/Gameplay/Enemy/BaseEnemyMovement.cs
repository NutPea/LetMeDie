using System;
using UnityEngine;

public class BaseEnemyMovement : MonoBehaviour
{
    private BaseEnemyController baseEnemyController;
    protected BaseEnemyController BaseEnemyController => baseEnemyController;
    private HealthManager healthManager;
    protected Transform player;
    private Animator animator;

    protected Rigidbody rb;
    protected RigidbodyConstraints beforeConstraints;


    private void Awake()
    {
        baseEnemyController = GetComponent<BaseEnemyController>();
        baseEnemyController.OnAggro.AddListener(OnAggro);
        baseEnemyController.OnKnockback.AddListener(Knockback);
        rb = GetComponent<Rigidbody>();

        animator = baseEnemyController.Animator;

        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(StopEverything);
    }



    protected virtual void StopEverything(GameObject died)
    {

    }

    protected void SetMovementAnimationValue(float value)
    {
        animator.SetFloat("Movement", value);
    }

    protected virtual void Start()
    {
        player = SGameManager.Instance.PlayerBody.transform;
    }

    protected virtual void OnAggro()
    {

    } 

    public virtual void StopMovement()
    {

    }

    public virtual void StartMovement()
    {

    }

    public virtual void Knockback(Vector3 dir)
    {
        Debug.Log("Knockback" + dir);

        SimpleRigidbodyKnockback(dir);
    }

    protected void SimpleRigidbodyKnockback(Vector3 dir)
    {
        beforeConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.AddForce(dir, ForceMode.Impulse);
        Invoke(nameof(ResetKnockback), BaseEnemyController.EnemyData.KnockBackResetTime);
    }

    public virtual void ResetKnockback()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = beforeConstraints;
    }

    

}
