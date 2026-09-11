using UnityEngine;

[CreateAssetMenu(fileName = "LevelChargeSpeedBuffBattleLoot", menuName = "BattleLoot/Buff/CharacterBuff/LevelChargeSpeedBuffBattleLoot", order = 1)]
public class LevelChargeSpeedBuffBattleLoot : LevelBuffBattleLoot
{
    [SerializeField] private float levelUpChargeSpeedAmount = 0.02f;
    float lastLevelUpMovementAmount = 0.0f;
    public override void UpdateBuffBattleLoot(GameObject player, PlayerData data)
    {
        base.UpdateBuffBattleLoot(player, data);

        data.ExtraChargeSpeedPercentage -= lastLevelUpMovementAmount;
        float levelUpChargeSpeedAmount = data.CurrentLevel * this.levelUpChargeSpeedAmount;

        data.ExtraChargeSpeedPercentage += levelUpChargeSpeedAmount;
        lastLevelUpMovementAmount = levelUpChargeSpeedAmount;
        playerData.OnStatUpdate.Invoke();

    }

    public override void ResetLevelBuff(GameObject player, PlayerData data)
    {
        lastLevelUpMovementAmount = 0;
    }
}
