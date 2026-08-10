using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponEquiper : MonoBehaviour
{

    private PlayerWeaponController playerWeaponController;
    private PlayerStatHandler playerStatHandler;
    private PlayerCombatController playerCombatController;
    [HideInInspector] public UnityEvent<WeaponData> OnEquipWeapon = new();
    [HideInInspector] public UnityEvent<WeaponData> OnEquipSpell_1 = new();
    [HideInInspector] public UnityEvent<WeaponData> OnEquipSpell_2 = new();

    [Header("Test")]
    [SerializeField] private bool useTestWeapon;
    [SerializeField] private WeaponData testWeapon;

    private void Awake()
    {
        playerWeaponController = GetComponent<PlayerWeaponController>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerCombatController = GetComponent<PlayerCombatController>();  
    }

    void Start()
    {
        if (useTestWeapon)
        {
            EquipWeapon(testWeapon);
        }
        else
        {
            EquipWeapon(playerStatHandler.PlayerData.CurrentEquipedWeapon);
        }
    }

    public void EquipWeapon(WeaponData data)
    {
        playerWeaponController.EquipWeapon(data);
        OnEquipWeapon.Invoke(data);
        playerCombatController.CurrentFullChargeTime = data.FullChargeTime;
        playerStatHandler.PlayerData.CurrentEquipedWeapon = data;
        if(data is BowData bowData){
            playerCombatController.CanChangeFOVOnCharge = true;
        }
        else{
            playerCombatController.CanChangeFOVOnCharge = false;
        }
    }



    public void EquipSpell1(MagicSpell magicSpell)
    {
        playerStatHandler.PlayerData.CurrentMagicSpell_1 = magicSpell;
        OnEquipSpell_1.Invoke(magicSpell);
        playerWeaponController.EquipMagicSpell(magicSpell);
    }

    public void EquipSpell2(MagicSpell magicSpell)
    {
        playerStatHandler.PlayerData.CurrentMagicSpell_2 = magicSpell;
        OnEquipSpell_2.Invoke(magicSpell);
        playerWeaponController.EquipMagicSpell(magicSpell);
    }

}
