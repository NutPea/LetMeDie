using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item/HealthPotion", order = 1)]
public class HealthPotionItemData : ConsumbaleData
{
    [SerializeField] private int healAmount;
    private HealthManager _playerHealthManager;




    public override void Use(GameObject player)
    {
        if(_playerHealthManager == null)
        {
            _playerHealthManager = player.GetComponent<HealthManager>();
        }
        _playerHealthManager.Heal(healAmount);
        base.Use(player);
    }
}
