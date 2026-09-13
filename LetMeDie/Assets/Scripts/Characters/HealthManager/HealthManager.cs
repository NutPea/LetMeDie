using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CalculateDamageEvent : UnityEvent<bool,int,Transform>
{

}


public class HealthManager : MonoBehaviour
{

    public HealthData healthData;

    public CalculateDamageEvent OnDamaged;
    [HideInInspector] public UnityEvent OnHeal = new UnityEvent();
    [HideInInspector] public UnityEvent<List<CombatEffect>> OnCombatEffect = new();
    [HideInInspector]public UnityEvent<GameObject> OnDeath = new();
    [HideInInspector] public UnityEvent OnHealthUpdate = new UnityEvent();

    public bool IsFullHealth => healthData.CurrentHealth >= healthData.Health;
    public float CurrentPercentageHealth => (float)healthData.CurrentHealth / (float) healthData.Health;

    [HideInInspector] public bool CanBlock = true;
    [HideInInspector] public bool IsBlocked = false;
    [HideInInspector] public UnityEvent<int> OnDamageBlocked = new();

    protected bool isInvincible;

    private void Awake()
    {
        OnDamaged = new CalculateDamageEvent();
    }

    protected virtual void Start()
    {
        healthData.InitHealth();
    }



    private void OnDisable()
    {
        OnDamaged.RemoveAllListeners();
    }


    public void InflictDamage(int damage,List<CombatEffect> combatEffects ,TeamFlag team, Transform hitSource)
    {
        InflictDamage(damage,team,hitSource);
        OnCombatEffect.Invoke(combatEffects);

    }

    public virtual void InflictDamage(int damage,TeamFlag team,Transform hitSource)
    {
        if (isInvincible)
        {
            return;
        }

        int appliedDamage = damage;
        bool isDead = false;
        if (IsBlocked)
        {
            if (CanBlock)
            {
                OnDamageBlocked.Invoke(1);
                return;
            }
        }

        if(team != healthData.team)
        {
            if (healthData.CurrentHealth <= 0) return;
            healthData.CurrentHealth -= appliedDamage;
            if (SGameManager.Instance.ShouldShowDamageDumber)
            {
                if(healthData.team != TeamFlag.Player)
                {
                    SGameManager.Instance.ShowDamageNumber(transform, damage);
                }
            }

            if(healthData.CurrentHealth <= 0)
            {
                healthData.CurrentHealth = 0;
                OnDeath.Invoke(gameObject);
                isDead = true;
                OnDamaged.Invoke(true, appliedDamage, hitSource);
            }
            else
            {
                OnDamaged.Invoke(false, appliedDamage, hitSource);
            }
        }

        if(healthData.team == TeamFlag.Enemy)
        {
            SGameManager.Instance.OnEnemyDamage.Invoke(this, isDead);
        }
    }

    public void Heal(int amount)
    {
        if (IsFullHealth){
            return;
        }

        healthData.CurrentHealth += amount;
        if(healthData.CurrentHealth >= healthData.Health)
        {
            healthData.CurrentHealth = healthData.Health;
        }
        OnHeal.Invoke();
    }

    public void FullHeal()
    {
        healthData.CurrentHealth = healthData.Health;
        OnHeal.Invoke();
    }

    public virtual void Recover()
    {
        FullHeal();
    }



    public void Kill()
    {
        if (healthData.CurrentHealth <= 0) return;
        healthData.CurrentHealth -= 100000;
        healthData.CurrentHealth = 0;
        OnDeath.Invoke(gameObject);
        OnDamaged.Invoke(true, 10, transform);
    }


}
