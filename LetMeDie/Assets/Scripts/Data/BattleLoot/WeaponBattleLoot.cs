using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBattleLoot", menuName = "BattleLoot/WeaponBattleLoot", order = 1)]
public class WeaponBattleLoot : BattleLoot
{
    [SerializeField] private WeaponData weaponData;
    public WeaponData WeaponData => weaponData;

    public override string Name => weaponData.ItemName;
    public override string Description => "Equips " + weaponData.ItemName;


    public override Sprite Icon => weaponData.Sprite;

    public override Color Tint => weaponData.Tint;
}
