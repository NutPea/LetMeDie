using UnityEngine;

public class RagePowerUp : PowerUp
{
    [SerializeField] private RageBuff rageBuff;
    protected override void PickUp(GameObject player)
    {
        Debug.Log("PickUp");
        player.GetComponent<PlayerStatHandler>().PlayerData.AddTempBuffBattleLoot(rageBuff);
    }
}
