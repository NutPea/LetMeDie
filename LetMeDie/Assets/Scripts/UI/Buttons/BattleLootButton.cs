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

    private BattleLoot currentBattleLoot;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnChooseBattle.Invoke(currentBattleLoot));
    }

    public void SetBattleLoot(BattleLoot battleLoot)
    {
        rarityColor.color = SGameManager.Instance.GetRarityColor(battleLoot.lootRarity);
        head.text = battleLoot.Name;
        iconImage.sprite = battleLoot.Icon;
        iconImage.color = battleLoot.Tint;
        if(battleLoot is BuffBattleLoot buff)
        {
            description.text = buff.BeforeUpgradeValue +" >>> " + buff.AfterUpgradeValue;
        }
        else
        {
            description.text = battleLoot.Description;
        }

        currentBattleLoot = battleLoot;
    }


}
