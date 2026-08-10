using UnityEngine;
using UnityEngine.Events;

public class MagicSpell : WeaponData
{
    [SerializeField] private int spellManaCost = 10;
    public int SpellManaCost => spellManaCost;
    protected PlayerResourceHandler playerResourceHandler;
    public UnityEvent<MagicSpell> OnSpellCast = new();

    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        playerResourceHandler = playerWeaponController.GetComponent<PlayerResourceHandler>();
        Debug.Log(this + "Equip");
    }

    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack(camera, chargeAmount);
        if (playerResourceHandler.CurrentMana >= SpellManaCost) {
            Cast(camera);
            playerResourceHandler.UseMana(SpellManaCost);
            OnSpellCast.Invoke(this);
        }
        
    }

    public virtual void Cast(Transform camera)
    {

    }

}
