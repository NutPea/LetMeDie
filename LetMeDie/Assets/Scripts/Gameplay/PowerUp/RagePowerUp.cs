using Essentials;
using UnityEngine;

public class RagePowerUp : PowerUp
{
    [SerializeField] private RageBuff rageBuff;
    protected override void PickUp(GameObject player)
    {
        player.GetComponent<PlayerStatHandler>().PlayerData.AddTempBuffBattleLoot(rageBuff);
        SUIManager.Instance.GetUIState("Game").UIStateObject.GetComponent<GameUIStateComponent>().ShowRageBorder(rageBuff.TemporaryBuffTime);
    }
}
