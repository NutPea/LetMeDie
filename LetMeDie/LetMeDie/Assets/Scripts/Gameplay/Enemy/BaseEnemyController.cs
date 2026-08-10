using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(HealthManager))]
public class BaseEnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private EnemyData copyedEnemyData;
    [SerializeField] private bool editEnemyDataInPlaymode;

    public EnemyData EnemyData { get 
        {
            if (editEnemyDataInPlaymode)
            {
                return enemyData;
            }

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
    private BaseEnemyMovement baseEnemyMovement;

    [HideInInspector] public UnityEvent OnAggro = new();
    //Enemy Combat Controller Invokes this
    [HideInInspector] public UnityEvent<Vector3> OnKnockback = new();
    private bool isAggro;
    public bool IsAggro => isAggro;

    [SerializeField] private float staggerPercantage = 0.3f;


    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.healthData = EnemyData;
        baseEnemyMovement = GetComponent<BaseEnemyMovement>();
    }
    private void Start()
    {
        healthManager.OnDamaged.AddListener(GotHit);
        healthManager.OnDeath.AddListener(Death);
    }

    private void Death(GameObject arg0)
    {
        baseEnemyMovement.StopMovement();
    }

    private void GotHit(bool died, int damageAmount, float knockBack, Transform arg2)
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
