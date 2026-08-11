using System;
using UnityEngine;

public class BaseEnemyMovement : MonoBehaviour
{
    private BaseEnemyController baseEnemyController;
    public BaseEnemyController BaseEnemyController => baseEnemyController;
    private HealthManager healthManager;
    protected Transform player;
    private Animator animator;

    protected Rigidbody rb;
    protected RigidbodyConstraints beforeConstrain;


    private void Awake()
    {
        baseEnemyController = GetComponent<BaseEnemyController>();
        baseEnemyController.OnAggro.AddListener(OnAggro);

        animator = baseEnemyController.Animator;

        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(StopEverything);

        rb = GetComponent<Rigidbody>();
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


    public virtual void Knockback(Vector3 knockBackVector, float knockBackLegth)
    {
        Knockback(knockBackVector);
        Invoke(nameof(ResetAfterKnockBack), knockBackLegth);
    }

    public virtual void ResetAfterKnockBack()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = beforeConstrain;
    }

    protected void Knockback(Vector3 knockBackVector)
    {
        beforeConstrain = rb.constraints;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        rb.AddForce(knockBackVector,ForceMode.Impulse);
    }


}
