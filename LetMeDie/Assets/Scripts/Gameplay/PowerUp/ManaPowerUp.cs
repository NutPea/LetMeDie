using Essentials;
using UnityEngine;

public class ManaPowerUp : PowerUp
{
    [SerializeField] private int manaRecoverAmount = 100;

    protected override void PickUp(GameObject player)
    {
        player.GetComponent<PlayerStatHandler>().PlayerData.RegSpellMana(manaRecoverAmount);
        SUIManager.Instance.GetUIState("Game").UIStateObject.GetComponent<GameUIStateComponent>().ShowManaBorder(0.5f);
    }
}
