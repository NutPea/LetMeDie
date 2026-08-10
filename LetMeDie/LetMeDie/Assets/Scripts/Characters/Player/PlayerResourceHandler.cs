using UnityEngine;
using UnityEngine.Events;

public class PlayerResourceHandler : HealthManager
{

    private PlayerData data;
    public int CurrentMana;

    public bool HasEnoughMana => CurrentMana > 0;
    public bool ManaIsFull => CurrentMana == data.Mana;
    public UnityEvent<float> OnManaChanged = new UnityEvent<float>();
    private float ManaPercentage => (float)CurrentMana / (float) data.Mana;


    public void SetData(PlayerData playerData)
    {
        this.healthData = playerData;
        data = playerData;
        currentHealth = healthData.Health;
        OnHealthUpdate.Invoke();
        RegMana(playerData.Mana);
    }

    public void UseMana(int amount)
    {
        CurrentMana -= amount;
        if(CurrentMana< 0)
        {
           CurrentMana = 0;
        }
        OnManaChanged.Invoke(ManaPercentage);
    }

    public void RegMana(int mana)
    {
        CurrentMana += mana;
        if (CurrentMana > data.Mana)
        {
            CurrentMana = data.Mana;
        }
        OnManaChanged.Invoke(ManaPercentage);
    }

    public void FullManaReg()
    {
        CurrentMana = data.Mana;
    }

    public override void Recover()
    {
        base.Recover();
        FullManaReg();
    }
}
