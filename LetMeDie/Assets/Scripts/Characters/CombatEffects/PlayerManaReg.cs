using Essentials;
using UnityEngine;


[CreateAssetMenu(menuName = "CombatEffect/Player/ManaReg", fileName = "ManaReg")]
public class PlayerManaReg : CombatEffect
{
    [SerializeField] private int manaRegenerationAmount = 1;
    private PlayerData playerData;

    public override void Init(Transform offender)
    {
        base.Init(offender);
        playerData = offender.GetComponent<PlayerStatHandler>().PlayerData;
    }

    public override void ResolveCombatEffect(Transform victim)
    {
        base.ResolveCombatEffect(victim);
        playerData.RegSpellMana(manaRegenerationAmount);
    }

}
