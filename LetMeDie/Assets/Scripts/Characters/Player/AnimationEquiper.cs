using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEquiper : MonoBehaviour
{
    [SerializeField] private List<WeaponAnimationHelper> weaponAnimationHelper = new();
    private Dictionary<string,List<GameObject>> animationDictonary = new Dictionary<string,List<GameObject>>();
    [SerializeField] private PlayerWeaponEquiper weaponEquiper;
    [SerializeField] private PlayerVfXAttackController vfxController;

    private void Awake()
    {
        foreach (WeaponAnimationHelper weapon in weaponAnimationHelper) {
            animationDictonary.Add(weapon.data.GUID, weapon.weaponParts);
        }
        weaponEquiper.OnEquipWeapon.AddListener(Equip);
    }

    public void Equip(WeaponData data)
    {
        RemoveAllWeapons();

        vfxController.SetWeaponData(data);
        List<GameObject> weaponParts = animationDictonary[data.GUID];

        if (data is SwordData swordData)
        {
            weaponParts[0].SetActive(true);
            vfxController.SetSwordWeapon(weaponParts[0].GetComponent<WeaponHandler>(), swordData);
        }
        else if(data is BowData bowData)
        {
            vfxController.HideTrail();
            foreach (GameObject parts in weaponParts)
            {
                parts.SetActive(true);
            }
        }
        else
        {
            vfxController.HideTrail();
        }
    }


    private void RemoveAllWeapons()
    {
        foreach(List<GameObject> weapons in animationDictonary.Values)
        {
            foreach (GameObject weaponParts in weapons) {
                weaponParts.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class WeaponAnimationHelper
{
    public List<GameObject> weaponParts;
    public WeaponData data;
}
