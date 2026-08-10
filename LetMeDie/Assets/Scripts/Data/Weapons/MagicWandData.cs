using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/MagicWand", order = 1)]
public class MagicWandData : SwordData
{
    [SerializeField] private float percentageManaRecovery = 0.1f;
    [SerializeField] private float timeUntilRecovery = 1f;

    private float currentRecoveryTime;
    private PlayerResourceHandler playerResourceHandler;

    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        currentRecoveryTime = timeUntilRecovery;
        playerResourceHandler = playerWeaponController.GetComponent<PlayerResourceHandler>();
    }

    public override void Update()
    {
        base.Update();

        if (playerResourceHandler.ManaIsFull)
        {
            return;
        }
        if(currentRecoveryTime < 0)
        {
            Recovery();
            currentRecoveryTime = timeUntilRecovery;
        }
        currentRecoveryTime -= Time.deltaTime;
    }

    private void Recovery()
    {
        int toRecoverMana = Mathf.RoundToInt((float)playerData.Mana * percentageManaRecovery);
        playerResourceHandler.RegMana(toRecoverMana);
    }


}
