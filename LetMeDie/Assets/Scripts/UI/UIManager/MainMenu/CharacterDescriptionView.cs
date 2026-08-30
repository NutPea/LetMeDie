using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDescriptionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterImage;

    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponDescription;

    [SerializeField] private Image buffImage;
    [SerializeField] private TextMeshProUGUI buffName;
    [SerializeField] private TextMeshProUGUI buffDescription;

    public void ShowCharacter(PlayerData playerData)
    {
        characterName.text = playerData.Name;
        characterImage.sprite = playerData.Icon;

        weaponImage.sprite = playerData.CurrentEquipedWeapon.Sprite;
        weaponName.text = playerData.CurrentEquipedWeapon.ItemName;
        weaponDescription.text = playerData.CurrentEquipedWeapon.Description;

        if (playerData.CharacterBuffBattleLoot != null)
        {
            buffImage.gameObject.SetActive(true);
            buffName.gameObject.SetActive(true);
            buffImage.sprite = playerData.CharacterBuffBattleLoot.Icon;
            buffName.text = playerData.CharacterBuffBattleLoot.Name;
            buffDescription.text = playerData.CharacterBuffBattleLoot.Description;
        }
        else
        {
            buffImage.gameObject.SetActive(false);
            buffName.gameObject.SetActive(false);
        }

    }
}
