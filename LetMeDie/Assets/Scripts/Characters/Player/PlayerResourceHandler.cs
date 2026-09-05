using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PixelCrushers.AnimatorSaver;

public class PlayerResourceHandler : HealthManager
{

    private PlayerData playerData;
    public int CurrentStamina = 0;

    public bool HasEnoughStamina => CurrentStamina > 0;
    public bool StaminaIsFull => CurrentStamina == playerData.Stamina;

    public bool StaminaIsEmpty => CurrentStamina <= 0;

    public UnityEvent<int> OnStaminaChanged = new UnityEvent<int>();

    private float currentRegenerationTime = 0;

    private const float STAT_REGENERATION_TIME = 1;

    private float currentStatRegenerationTime = 0;

    private float manaRegValue;
    private float healthRegValue;

    private float lifeStealAmount;


    private void Start()
    {
        currentRegenerationTime = playerData.StaminaRegeneration;
        OnDamageBlocked.AddListener(UseStamina);
        CurrentStamina = playerData.Stamina;
        currentStatRegenerationTime = STAT_REGENERATION_TIME;
        OnDamaged.AddListener(SetInvincible);
    }

    private void SetInvincible(bool arg0, int arg1, Transform arg2)
    {
        isInvincible = true;
        CancelInvoke(nameof(ResetInvincible));
        Invoke(nameof(ResetInvincible), playerData.InvincibleTime);
    }

    private void ResetInvincible()
    {
        isInvincible = false;
    }

    private void Update()
    {

        if (!StaminaIsFull)
        {
            if (currentRegenerationTime < 0)
            {
                RegStamina(1);
                currentRegenerationTime = playerData.StaminaRegeneration;
            }
            else
            {
                currentRegenerationTime -= Time.deltaTime;
            }
        }

        if(currentRegenerationTime < 0)
        {
            currentRegenerationTime = STAT_REGENERATION_TIME;
            if(playerData.SpellManaRegeneration > 0)
            {
                manaRegValue += playerData.SpellManaRegeneration;
                if(manaRegValue > 1)
                {
                    int toManaRegValue = Mathf.CeilToInt(manaRegValue);
                    manaRegValue -= toManaRegValue;
                    for (int i = 0; i < toManaRegValue; i++) {
                        playerData.RegSpellMana();
                    }
                }

            }

            if(playerData.HealthRegRate > 0)
            {
                healthRegValue += playerData.HealthRegRate;
                if(healthRegValue > 1)
                {
                    int toHealReg = Mathf.CeilToInt(healthRegValue);
                    healthRegValue -= toHealReg;
                    Heal(toHealReg);
                }
            }
        }
        else
        {
            currentRegenerationTime -= Time.deltaTime;
        }
    }

    public override void InflictDamage(int damage, TeamFlag team, Transform hitSource)
    {
        float evasionPercentage = UnityEngine.Random.Range(0.0f, 1.0f);
        if(evasionPercentage < playerData.Evasion)
        {
            return;
        }
        base.InflictDamage(damage, team, hitSource);
    }

    public void SetData(PlayerData playerData)
    {
        this.healthData = playerData;
        this.playerData = playerData;
        currentHealth = healthData.Health;
        OnHealthUpdate.Invoke();
    }

    public void UseStamina(int amount)
    {
        CurrentStamina -= amount;
        if(CurrentStamina <= 0)
        {
            CurrentStamina = 0;
            CanBlock = false;
        }
        OnStaminaChanged.Invoke(CurrentStamina);
    }

    public void RegStamina(int amount)
    {
        CurrentStamina += amount;

        if (CurrentStamina > playerData.Stamina)
        {
            CurrentStamina = playerData.Stamina;
        }
        if(CurrentStamina > 0)
        {
            CanBlock = true;
        }
        OnStaminaChanged.Invoke(CurrentStamina);
    }

    

    public void FullManaReg()
    {
        CurrentStamina = playerData.Stamina;
    }

    public override void Recover()
    {
        base.Recover();
        FullManaReg();
    }

    internal void LifeSteal(int amount, float baseLifeSteal)
    {
        if(baseLifeSteal > 0)
        {
            lifeStealAmount += (float)amount * baseLifeSteal;
            if(lifeStealAmount > 1)
            {
                int potentialHeal = (int)lifeStealAmount;
                lifeStealAmount -= potentialHeal;
                Heal(potentialHeal);
            }
        }
    }
}
