using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HealthData" ,fileName = "HealthData")]
public class HealthData : CharacterData
{
    [SerializeField] protected int baseHealth = 100;
    public virtual int Health => baseHealth;

    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set {
            currentHealth = value; }
    }

    public TeamFlag team;

    private bool hasBeenInit;
    public void InitHealth()
    {
        if (!hasBeenInit)
        {
            currentHealth = Health;
            hasBeenInit = true;
        }
    }
}
