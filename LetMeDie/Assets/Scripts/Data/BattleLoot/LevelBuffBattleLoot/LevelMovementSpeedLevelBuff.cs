using UnityEngine;


[CreateAssetMenu(fileName = "LevelMovementSpeedLevelBuff", menuName = "BattleLoot/Buff/CharacterBuff/LevelMovementSpeedLevelBuff", order = 1)]
public class LevelMovementSpeedLevelBuff : LevelBuffBattleLoot
{
    [SerializeField] private float levelUpMovementAmount = 0.02f;
    float lastLevelUpMovementAmount = 0.0f;
    public override void UpdateBuffBattleLoot(GameObject player, PlayerData data)
    {
        base.UpdateBuffBattleLoot(player, data);

        data.ExtraMovementSpeedPercent -= lastLevelUpMovementAmount;
        float levelUpMovementAmount = data.CurrentLevel * this.levelUpMovementAmount;

        data.ExtraMovementSpeedPercent += levelUpMovementAmount;
        lastLevelUpMovementAmount = levelUpMovementAmount;

    }
}
