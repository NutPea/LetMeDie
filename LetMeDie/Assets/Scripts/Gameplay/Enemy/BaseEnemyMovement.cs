using System;
using UnityEngine;

public class BaseEnemyMovement : MonoBehaviour
{
    private BaseEnemyController baseEnemyController;
    protected BaseEnemyController BaseEnemyController => baseEnemyController;
    private HealthManager healthManager;
    protected Transform player;
    private Animator animator;

    private void Awake()
    {
        baseEnemyController = GetComponent<BaseEnemyController>();
        baseEnemyController.OnAggro.AddListener(OnAggro);

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


}
