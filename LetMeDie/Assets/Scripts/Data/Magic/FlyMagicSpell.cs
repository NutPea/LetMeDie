using UnityEngine;

[CreateAssetMenu(fileName = "Fly", menuName = "Weapons/Magic/SpecialSpell/Fly", order = 1)]
public class FlyMagicSpell : MagicSpell
{
    private PlayerCharacterControllerMovementController movementController;
    [SerializeField] private float dashSpeed = 1.0f;
    [SerializeField] private float dashDuration = 1.0f;
    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        movementController = playerWeaponController.GetComponent<PlayerCharacterControllerMovementController>();
    }

    public override void Cast(Transform camera)
    {
        base.Cast(camera);
        movementController.ForceDash(camera.forward, dashSpeed, dashDuration);
    }

}
