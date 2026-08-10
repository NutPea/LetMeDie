using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HealthData" ,fileName = "HealthData")]
public class HealthData : CharacterData
{
    [SerializeField] protected int baseHealth = 100;
    public virtual int Health => baseHealth;

    public TeamFlag team;
}
