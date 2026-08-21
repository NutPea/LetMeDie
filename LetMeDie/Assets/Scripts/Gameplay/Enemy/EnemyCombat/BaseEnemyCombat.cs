using System;
using System.Collections.Generic;
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
    private EnemyData enemyData;
    private bool isDead;
    private bool canAttack;

    private EnemyAttackData currentEnemyAttackData;
    private List<EnemyAttackData> attackDataOnCooldown = new();

    private void Start()
    {
        baseEnemyController = GetComponent<BaseEnemyController>();
        baseEnemyController.OnAggro.AddListener(PickAttack);
        baseEnemyMovement = GetComponent<BaseEnemyMovement>();
        enemyData = baseEnemyController.EnemyData;

        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(StopEverything);

        enemyAnmationHelper.OnAnimationAttack.AddListener(Attack);
        enemyAnmationHelper.OnAntizipationAttack.AddListener(AttackAntizipation);

        player = SGameManager.Instance.PlayerBody.transform;
        PickAttack(); 
    }


    private void PickAttack()
    {
        if (currentEnemyAttackData != null) {
            currentEnemyAttackData.AttackFinished(this);
        }
        if (enemyData.EnemyAttackDatas.Count <= 0) {
            return;
        }
        currentEnemyAttackData = enemyData.GetAttack();
        currentEnemyAttackData.Select(this, player);
        canAttack = true;
        currentEnemyAttackData.SetCooldown(AddToCooldownList);
    }

    private void AddToCooldownList(EnemyAttackData data)
    {
        attackDataOnCooldown.Add(data);
    }

    public virtual void Stagger(float time)
    {
        BaseEnemyController.Animator.SetTrigger("Hit");
        if(currentEnemyAttackData != null)
        {
            currentEnemyAttackData.AttackFinished(this);
            Invoke(nameof(PickAttack), time);
        }
    }


    private void StopEverything(GameObject diedObject)
    {
        
        BaseEnemyController.Animator.SetBool("Death",true);
        BaseEnemyController.Animator.SetTrigger("DeathTrigger");
        isDead = true;
    }

    private void Update()
    {
        if (attackDataOnCooldown.Count > 0)
        {
            EnemyAttackData removeData = null;
            foreach (EnemyAttackData enemyAttackData in attackDataOnCooldown)
            {
                enemyAttackData.UpdateCooldown();
                if (!enemyAttackData.IsOnCooldown)
                {
                    enemyAttackData.ResetCooldown();
                    removeData = enemyAttackData;
                }
            }
            if (removeData != null)
            {
                attackDataOnCooldown.Remove(removeData);
            }
        }

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
