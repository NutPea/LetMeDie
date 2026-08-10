using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Image image;

    public void SetItem(ItemData item)
    {
        if (item is ConsumbaleData consumable)
        {
            SetConsumable(consumable);
        }
        else if(item is WeaponData weaponData)
        {
            SetWeapon(weaponData);
        }
        else
        {
            header.text = "---";
            description.text = "---";
            image.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }
    }

    private void SetWeapon(WeaponData weaponData)
    {
        image.gameObject.SetActive(true);
        amount.gameObject.SetActive(false);
        header.text = weaponData.ItemName;
        description.text = weaponData.Description;
        image.sprite = weaponData.Sprite;
        image.color = weaponData.Tint;
    }

    private void SetConsumable(ConsumbaleData consumbaleData)
    {
        image.gameObject.SetActive(true);
        amount.gameObject.SetActive(true);
        header.text = consumbaleData.ItemName;
        description.text = consumbaleData.Description;
        image.sprite = consumbaleData.Sprite;
        image.color = consumbaleData.Tint;
        amount.text = "x" + consumbaleData.Amount.ToString();
    }

}
