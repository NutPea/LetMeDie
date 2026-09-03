using UnityEngine;


[CreateAssetMenu(fileName = "LevelManaReg", menuName = "BattleLoot/Buff/CharacterBuff/LevelManaReg", order = 1)]
public class LevelManaRegCharacterBuffBattleLoot : LevelBuffBattleLoot
{
    [SerializeField] private float manaRegAmount = 0.02f;
    float lastManaRegAmount = 0.0f;
    public override void UpdateBuffBattleLoot(GameObject player, PlayerData data)
    {
        base.UpdateBuffBattleLoot(player, data);

        data.SpellManaRegeneration -= lastManaRegAmount;
        float manaRegAmount = data.CurrentLevel * this.manaRegAmount;

        data.SpellManaRegeneration += manaRegAmount;
        lastManaRegAmount = manaRegAmount;

    }
}
