using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BaseEnemyCombat : MonoBehaviour
{
    [SerializeField] private EnemyAnmationEventHelper enemyAnmationHelper;
    private HealthManager healthManager;
    private BaseEnemyController baseEnemyController;
    public BaseEnemyController BaseEnemyController => baseEnemyController;
    private BaseEnemyMovement baseEnemyMovement;
    public BaseEnemyMovement BaseEnemyMovement => baseEnemyMovement;
    


    private Transform player;
    private Rigidbody rb;
    private EnemyData enemyData;
    private bool isDead;
    private bool canAttack;
    private bool isStaggered;

    private EnemyAttackData currentEnemyAttackData;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        baseEnemyController = GetComponent<BaseEnemyController>();
        baseEnemyController.OnAggro.AddListener(PickAttack);
        baseEnemyMovement = GetComponent<BaseEnemyMovement>();
        enemyData = baseEnemyController.EnemyData;

        healthManager = GetComponent<HealthManager>();
        healthManager.OnDamaged.AddListener(OnHit);
        healthManager.OnDeath.AddListener(StopEverything);

        enemyAnmationHelper.OnAnimationAttack.AddListener(Attack);
        enemyAnmationHelper.OnAntizipationAttack.AddListener(AttackAntizipation);

        player = SGameManager.Instance.PlayerBody.transform;
        PickAttack();
    }

    private void OnHit(bool arg0, int arg1, float knockBack, Transform arg2)
    {
        if (isStaggered)
        {
            return;
        }

        //For now simplicity vise knockback and stagger are the same
        if (BaseEnemyController.EnemyData.CalculateKnockbackPossibility(knockBack))
        {
            if (currentEnemyAttackData != null)
            {
                currentEnemyAttackData.AttackFinished(this);
            }
            BaseEnemyController.Animator.SetBool("Staggered", true);
            BaseEnemyController.Animator.SetTrigger("Hit");
            isStaggered = true;
            Invoke(nameof(PickNewAttackAfterStagger), enemyData.StaggerTime);

            Vector3 dir = transform.position - arg2.transform.position;
            dir.y = 0;
            dir = dir.normalized;
            float knockBackStregth = BaseEnemyController.EnemyData.CalculateKnockbackStrength(knockBack);
            dir *= BaseEnemyController.EnemyData.CalculateKnockbackStrength(knockBack);
            BaseEnemyController.OnKnockback.Invoke(dir);
        }
    }

    private void PickNewAttackAfterStagger()
    {
        BaseEnemyController.Animator.SetBool("Staggered", false);
        isStaggered = false;
        PickAttack();
    }

    private void PickAttack()
    {
        if (!BaseEnemyController.EnemyData.HasAttacks)
        {
            return;
        }

        if (currentEnemyAttackData != null) {
            currentEnemyAttackData.AttackFinished(this);
        }
        currentEnemyAttackData = enemyData.GetAttack();
        currentEnemyAttackData.Select(this, player);
        canAttack = true;
    }
    

    private void StopEverything(GameObject diedObject)
    {
        BaseEnemyController.Animator.SetBool("Death",true);
        BaseEnemyController.Animator.SetTrigger("DeathTrigger");
        isDead = true;
    }

    private void Update()
    {
        if (isDead || !BaseEnemyController.IsAggro || !canAttack)
        {
            return;
        }
        currentEnemyAttackData.AttackUpdate(this,player);
    }

    public void Attack()
    {
        if (currentEnemyAttackData != null) {
            currentEnemyAttackData.Attack(this);
            canAttack = false;
            Invoke(nameof(PickAttack),currentEnemyAttackData.AttackWaitTime);
        }
    }

    private void AttackAntizipation()
    {
        if (currentEnemyAttackData != null)
        {
            currentEnemyAttackData.AntizipationAttack(this);
        }
    }



}
