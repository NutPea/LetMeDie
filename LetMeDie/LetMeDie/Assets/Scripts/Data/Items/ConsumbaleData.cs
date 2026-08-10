using UnityEngine;
using UnityEngine.Events;


public class ConsumbaleData : ItemData
{


    [SerializeField] private int amount;
    public int Amount => amount;

    [HideInInspector] public UnityEvent<ConsumbaleData> OnUse = new();
    [HideInInspector] public UnityEvent OnNoUsesLeft = new();

    public void AddConsumable(int amount)
    {
        this.amount += amount;
        OnUse.Invoke(this);
    }

    public virtual void Use(GameObject player)
    {
        amount--;
        if(amount <= 0)
        {
            amount = 0;
            OnNoUsesLeft.Invoke();
        }
        OnUse.Invoke(this);
    }
}
