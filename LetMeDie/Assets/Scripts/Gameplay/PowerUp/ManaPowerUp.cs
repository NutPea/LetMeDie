using UnityEngine;

public class ManaPowerUp : PowerUp
{
    [SerializeField] private int manaRecoverAmount = 100;

    protected override void PickUp(GameObject player)
    {
        player.GetComponent<PlayerStatHandler>().PlayerData.RegSpellMana(manaRecoverAmount);
    }
}
