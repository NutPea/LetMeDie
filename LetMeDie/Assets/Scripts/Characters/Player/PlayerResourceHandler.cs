using UnityEngine;
using UnityEngine.Events;

public class PlayerResourceHandler : HealthManager
{

    private PlayerData data;
    public int CurrentStamina = 0;

    public bool HasEnoughStamina => CurrentStamina > 0;
    public bool StaminaIsFull => CurrentStamina == data.Stamina;

    public bool StaminaIsEmpty => CurrentStamina <= 0;

    public UnityEvent<int> OnStaminaChanged = new UnityEvent<int>();

    private float currentRegenerationTime = 0;


    private void Start()
    {
        currentRegenerationTime = data.StaminaRegeneration;
        OnDamageBlocked.AddListener(UseStamina);
        CurrentStamina = data.Stamina;
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
}
