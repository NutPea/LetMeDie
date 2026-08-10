using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyCoreHolder : MonoBehaviour
{
    public bool infinitAmount;
    public int amountOfEnergyCores = 0;

    public void AddEnergyCore()
    {
        amountOfEnergyCores++;
    }

    public void RemoveEnergyCore()
    {
        amountOfEnergyCores--;
    }

    public bool HasEnoughEneryCores()
    {
        return amountOfEnergyCores > 0 || infinitAmount;
    }
}
