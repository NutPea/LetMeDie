using UnityEngine;

public class WeaponData : ItemData
{

    [SerializeField] protected int minDamageAmount;
    [SerializeField] protected int maxDamageAmount;
    [SerializeField] protected bool canBlock;
    [SerializeField] protected float minKnockBackStregth = 1;
    [SerializeField] protected float knockBackStregth = 0;
    
    public bool CanBlock => canBlock;
    [SerializeField] private float fullChargeTime = 0.5f;
    public float FullChargeTime => fullChargeTime;

    protected PlayerData playerData;
    protected PlayerWeaponController playerWeaponController;
    protected int attackLayer;

    [SerializeField] private float attackCooldown = 0.5f;
    public float AttackCooldown => attackCooldown;


    public virtual void Equip(PlayerWeaponController playerWeaponController)
    {
        attackLayer = ~LayerMask.GetMask("Player");
        playerData = playerWeaponController.PlayerData;
        this.playerWeaponController = playerWeaponController;
    }

    public virtual void Attack(Transform camera,float chargeAmount)
    {

    }

    public virtual void Update()
    {

    }

}
