using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleLootButton : MonoBehaviour
{
    [HideInInspector] public UnityEvent<BattleLoot> OnChooseBattle = new();

    [SerializeField] private TextMeshProUGUI head;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI description;


    [SerializeField] private Image rarityColor;
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    private BattleLoot currentBattleLoot;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnChooseBattle.Invoke(currentBattleLoot));
    }

    public void SetBattleLoot(BattleLoot battleLoot)
    {
        
        switch (battleLoot.lootRarity)
        {
            case BattleLoot.LootRarity.Common: rarityColor.color = commonColor; break;
            case BattleLoot.LootRarity.Uncommen: rarityColor.color = uncommonColor; break;
            case BattleLoot.LootRarity.Rare: rarityColor.color = rareColor; break;
            case BattleLoot.LootRarity.Epic: rarityColor.color = rareColor; break;
            case BattleLoot.LootRarity.Legendary: rarityColor.color = legendaryColor; break;
        }
        
        head.text = battleLoot.Name;
        iconImage.sprite = battleLoot.Icon;
        description.text = battleLoot.Description;
        currentBattleLoot = battleLoot;
    }


}
