using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(HealthManager))]
public class BaseEnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private EnemyData copyedEnemyData;
    public EnemyData EnemyData { get 
        { 
            if(copyedEnemyData == null)
            {
                copyedEnemyData = Instantiate(enemyData);
                copyedEnemyData.Init(this);
            }
            return copyedEnemyData; 
        } 
    }

    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    private HealthManager healthManager;

    [HideInInspector] public UnityEvent OnAggro = new();
    private bool isAggro;
    public bool IsAggro => isAggro;

    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.healthData = EnemyData;
        healthManager.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(GameObject death)
    {
        SGameManager.Instance.EnemyDied();
    }

    private void Start()
    {
        healthManager.OnDamaged.AddListener(GotHit);
        healthManager.OnCombatEffect.AddListener(ResolveCombatEffects);
    }

    private void ResolveCombatEffects(List<CombatEffect> combatEffects)
    {
       foreach(CombatEffect combatEffect in combatEffects)
       {
            combatEffect.ResolveCombatEffect(transform);
       }
    }

    private void GotHit(bool died, int damageAmount, Transform arg2)
    {
        if (died || isAggro) {
            return;
        }
    }

    public void SetAggro()
    {
        isAggro = true;
        OnAggro.Invoke();
    }
}
