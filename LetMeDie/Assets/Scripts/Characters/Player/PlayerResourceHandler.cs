using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PixelCrushers.AnimatorSaver;

public class PlayerResourceHandler : HealthManager
{

    private PlayerData data;
    public int CurrentStamina = 0;

    public bool HasEnoughStamina => CurrentStamina > 0;
    public bool StaminaIsFull => CurrentStamina == data.Stamina;

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
        currentRegenerationTime = data.StaminaRegeneration;
        OnDamageBlocked.AddListener(UseStamina);
        CurrentStamina = data.Stamina;
        currentStatRegenerationTime = STAT_REGENERATION_TIME;
        OnDamaged.AddListener(SetInvincible);
    }

    private void SetInvincible(bool arg0, int arg1, Transform arg2)
    {
        isInvincible = true;
        CancelInvoke(nameof(ResetInvincible));
        Invoke(nameof(ResetInvincible), data.InvincibleTime);
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
                currentRegenerationTime = data.StaminaRegeneration;
            }
            else
            {
                currentRegenerationTime -= Time.deltaTime;
            }
        }

        if(currentRegenerationTime < 0)
        {
            currentRegenerationTime = STAT_REGENERATION_TIME;
            if(data.SpellManaRegeneration > 0)
            {
                manaRegValue += data.SpellManaRegeneration;
                if(manaRegValue > 1)
                {
                    int toManaRegValue = Mathf.CeilToInt(manaRegValue);
                    manaRegValue -= toManaRegValue;
                    for (int i = 0; i < toManaRegValue; i++) {
                        data.RegSpellMana();
                    }
                }

            }

            if(data.HealthRegRate > 0)
            {
                healthRegValue += data.HealthRegRate;
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

    public void SetData(PlayerData playerData)
    {
        this.healthData = playerData;
        data = playerData;
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

        if (CurrentStamina > data.Stamina)
        {
            CurrentStamina = data.Stamina;
        }
        if(CurrentStamina > 0)
        {
            CanBlock = true;
        }
        OnStaminaChanged.Invoke(CurrentStamina);
    }

    

    public void FullManaReg()
    {
        CurrentStamina = data.Stamina;
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
