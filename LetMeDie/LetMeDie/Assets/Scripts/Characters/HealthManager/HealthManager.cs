using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;


public class CalculateDamageEvent : UnityEvent<bool,int,float,Transform>
{

}


public class HealthManager : MonoBehaviour
{

    public HealthData healthData;
    public int currentHealth;

    public CalculateDamageEvent OnDamaged;
    [HideInInspector] public UnityEvent OnHeal = new UnityEvent();
    [HideInInspector]public UnityEvent<GameObject> OnDeath = new();
    [HideInInspector] public UnityEvent OnHealthUpdate = new UnityEvent();

    public bool IsFullHealth => currentHealth >= healthData.Health;
    [HideInInspector] public bool isBlocked;
    public float CurrentPercentageHealth => (float) currentHealth / (float) healthData.Health;


    private void Awake()
    {
        OnDamaged = new CalculateDamageEvent();
    }

    private void Start()
    {
        currentHealth = healthData.Health;
    }



    private void OnDisable()
    {
        OnDamaged.RemoveAllListeners();
    }


    public void InflictDamage(int damage,float knockBackStrength, TeamFlag team,Transform hitSource)
    {
        int appliedDamage = damage;
        bool isDead = false;
        if (isBlocked)
        {
            appliedDamage = damage/2;
        }

        if(team != healthData.team)
        {
            if (currentHealth <= 0) return;
            currentHealth -= appliedDamage;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath.Invoke(gameObject);
                isDead = true;
                OnDamaged.Invoke(true, appliedDamage, knockBackStrength, hitSource);
            }
            else
            {
                OnDamaged.Invoke(false, appliedDamage, knockBackStrength, hitSource);
            }
        }

        if(healthData.team == TeamFlag.Enemy)
        {
            SGameManager.Instance.OnEnemyDamage.Invoke(this, isDead);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth >= healthData.Health)
        {
            currentHealth = healthData.Health;
        }
        OnHeal.Invoke();
    }

    public void FullHeal()
    {
        currentHealth = healthData.Health;
        OnHeal.Invoke();
    }

    public virtual void Recover()
    {
        FullHeal();
    }



    public void Kill()
    {
        if (currentHealth <= 0) return;
        currentHealth -= 100000;
        currentHealth = 0;
        OnDeath.Invoke(gameObject);
        OnDamaged.Invoke(true, 10,0, transform);
    }
    

}
