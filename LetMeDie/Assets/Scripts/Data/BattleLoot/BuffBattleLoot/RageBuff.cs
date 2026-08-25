using UnityEngine;

[CreateAssetMenu(fileName = "RageBuff", menuName = "BattleLoot/Buff/Temporary/RageBuff", order = 1)]
public class RageBuff : BuffBattleLoot
{
    [SerializeField] private float extraAttackSpeed = 0.5f;
    [SerializeField] private float extraDamage = 2.0f;
    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        this.playerData = data;
        data.ExtraAttackSpeed += extraAttackSpeed;
        Debug.Log("Add" + data.ExtraAttackSpeed);
        data.WeaponBaseDamagePercentage += extraDamage;
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        data.ExtraAttackSpeed -= extraAttackSpeed;
        data.WeaponBaseDamagePercentage -= extraDamage;
    }

}
