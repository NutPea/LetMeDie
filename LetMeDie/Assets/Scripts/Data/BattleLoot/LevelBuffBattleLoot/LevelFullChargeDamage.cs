using UnityEngine;

[CreateAssetMenu(fileName = "LevelFullChargeDamage", menuName = "BattleLoot/Buff/CharacterBuff/LevelFullChargeDamage", order = 1)]
public class LevelFullChargeDamage : LevelBuffBattleLoot
{

    [SerializeField] private float levelUpChargeDamageAmount = 0.02f;
    float lastLevelUpDamageAmount = 0.0f;
    public override void UpdateBuffBattleLoot(GameObject player, PlayerData data)
    {
        base.UpdateBuffBattleLoot(player, data);

        data.WeaponExtraChargeDamage -= lastLevelUpDamageAmount;
        float levelUpDamageAmount = data.CurrentLevel * levelUpChargeDamageAmount;

        data.WeaponExtraChargeDamage += levelUpDamageAmount;
        lastLevelUpDamageAmount = levelUpDamageAmount;

    }

}
