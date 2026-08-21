using System;
using UnityEngine;

public class EnemyAttackData : ScriptableObject
{

    [SerializeField] private float attackWaitTime = 1.0f;
    public float AttackWaitTime => attackWaitTime;


    [SerializeField] private float attackCooldown;
    private float currentAttackCooldown;

    private bool hasCooldown;
    public bool HasCooldown => hasCooldown;
    public bool IsOnCooldown => currentAttackCooldown > 0;


    protected EnemyData currentEnemyData;

    [Header("DEBUG")]
    [SerializeField] protected bool ShowDebug;

    public enum AnimationEnum
    {
        Attack1 = 0,
        Attack2 = 1,
        Attack3 = 2,
    }

    [SerializeField] private AnimationEnum currentAttackAnimation = AnimationEnum.Attack1;
    protected BaseEnemyController BaseEnemyController;

    public void SetCooldown(Action<EnemyAttackData> hasCooldownAction)
    {
        if(attackCooldown <= 0)
        {
            return;
        }
        currentAttackCooldown = attackCooldown;
        hasCooldown = true;
        hasCooldownAction.Invoke(this);
    }

    public void UpdateCooldown()
    {
        currentAttackCooldown -= Time.deltaTime;
    }

    public void ResetCooldown()
    {
        hasCooldown = false;
    }

    public virtual void Init(BaseEnemyController enemy,EnemyData enemyData)
    {
        currentEnemyData = enemyData;
        BaseEnemyController = enemy;
    }

    public virtual void Select(BaseEnemyCombat baseEnemyCombat, Transform Player)
    {

    }

    public virtual void AttackUpdate(BaseEnemyCombat baseEnemyCombat,Transform Player)
    {

    }

    public virtual void AntizipationAttack(BaseEnemyCombat baseEnemyCombat)
    {

    }

    public virtual void Attack(BaseEnemyCombat baseEnemyCombat)
    {

    }

    public virtual void AttackFinished(BaseEnemyCombat baseEnemyCombat)
    {

    }


    private string GetAttackAnimationString()
    {
        switch (currentAttackAnimation)
        {
            case AnimationEnum.Attack1: return "Attack1";
            case AnimationEnum.Attack2: return "Attack2";
            case AnimationEnum.Attack3: return "Attack3";
        }
        return "Attack1";
    }

    protected void PlayAttackAnimation()
    {
        BaseEnemyController.Animator.SetTrigger(GetAttackAnimationString());
    }

    protected void LookAtPlayer(Transform enemyTransform,Transform playerTransform,float lookAtSpeed)
    {
        Vector3 direction = playerTransform.position - enemyTransform.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        enemyTransform.rotation = Quaternion.Lerp(
            enemyTransform.rotation,
            targetRotation,
            lookAtSpeed * Time.deltaTime
        );
    }

}
