using System;
using UnityEngine;

public class CombatEffect : ScriptableObject
{
    [HideInInspector] public Transform Offender;

    public virtual void Init(Transform offender)
    {
        this.Offender = offender;
    }

    public virtual void ResolveCombatEffect(Transform victim)
    {

    }

    protected float CalculateKnockBackStregth(float knockBackStregth, float victimMass)
    {
        float knockBack = knockBackStregth - victimMass;
        if (knockBack < 0)
        {
            knockBack = 0;
        }
        return knockBack;
    }
}
