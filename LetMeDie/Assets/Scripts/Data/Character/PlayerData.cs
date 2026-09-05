
using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Data", menuName = "Character/PlayerData", order = 1)]
public class PlayerData : HealthData
{

    [SerializeField] private float baseInvincibleTime;
    public float InvincibleTime => baseInvincibleTime + baseInvincibleTime * extraInvinciblePerventage;

    private float extraInvinciblePerventage;
    public float ExtraInvinciblePerventage { set =>  extraInvinciblePerventage = value;}

    [Header("Movement")]

    [SerializeField] private float baseMovementSpeed = 5f;
    private float extraMovementSpeedPercent = 0.0f;
    public float ExtraMovementSpeedPercent { get { return extraMovementSpeedPercent; } set { extraMovementSpeedPercent = value; } }
    public float MovementSpeed => baseMovementSpeed + baseMovementSpeed * extraMovementSpeedPercent;

    [SerializeField] private float baseJumpForce = 5f;

    private float extraJumpSpeedPercent = 0.0f;
    public float ExtraJumpSpeedPercent { get { return extraJumpSpeedPercent; } set { extraJumpSpeedPercent = value; } }
    public float JumpForce => baseJumpForce + baseJumpForce * extraJumpSpeedPercent;

    [Header("Combat")]

    [SerializeField] private int baseStamina = 3;
    public int Stamina => baseStamina;

    public float baseStaminaReginaration = 1f;
    public float StaminaRegeneration => baseStaminaReginaration;
    public override int Health => baseHealth + extraHealth;

    private int extraHealth;
    public int ExtraHealth {  get { return extraHealth; } set { extraHealth = value; } }

    private float healthRegRate = 0;
    public float HealthRegRate { get { return healthRegRate; } set { healthRegRate = value; } }

    private float weaponBaseDamagePercentage = 0.0f;
    public float WeaponBaseDamagePercentage { get { return weaponBaseDamagePercentage; } set { weaponBaseDamagePercentage = value; } }

    private float spellBaseDamagePercentage = 0.0f;
    public float SpellBaseDamagePercentage { get { return spellBaseDamagePercentage; } set { spellBaseDamagePercentage = value; } }

    private float knockBackPercentage = 0.0f;
    public float KnockBackPercentage { get => knockBackPercentage; set => knockBackPercentage = value; }

    private float spellManaReduction = 0.0f;
    public float SpellManaReduction { get => spellManaReduction; set { spellManaReduction = value; } }

    private float spellManaRegeneration = 0.0f;
    public float SpellManaRegeneration { get => spellManaRegeneration; set { spellManaRegeneration = value; } }

    private float lifeStealPercentage = 0.0f;
    public float LifeStealPercentage { get => lifeStealPercentage; set => lifeStealPercentage = value; }

    [SerializeField] private float weaponChargeTime = 1.0f;
    public float WeaponChargeTime => weaponChargeTime - (Mathf.Lerp(0.0f, (weaponChargeTime / 2), extraAttackSpeedPercent));

    private float extraAttackSpeedPercent = 0.0f;

    public float ExtraAttackSpeed { get => extraAttackSpeedPercent; set { extraAttackSpeedPercent = value; } }

    private float weaponExtraChargeDamage = 0.0f;
    public float WeaponExtraChargeDamage { get => weaponExtraChargeDamage; set { weaponExtraChargeDamage = value; } }

    private float critChance = 0.0f;

    public float CritChance { get => critChance; set { critChance = value; } }

    private float extraCritDamage = 0.0f;

    public float ExtraCritDamage { get => extraCritDamage; set { extraCritDamage = value; } }

    private int luck;

    public int Luck { get => luck; set { luck = value; } }


    private float extraAttackSize = 0.0f;

    public float ExtraAttackSize { get => extraAttackSize; set { extraAttackSize = value; } }

    private float evasion = 0.0f;

    public float Evasion { get => evasion; set { evasion = value; } }

    private int extraAmountOfProjectiles = 0;

    public int ExtraAmountOfProjectiles { get => extraAmountOfProjectiles; set { extraAmountOfProjectiles = value; } }

    private float extraAmountOfProjectilesPercent = 0;

    public float ExtraAmountOfProjectilesPercent { get => extraAmountOfProjectilesPercent; set { extraAmountOfProjectilesPercent = value; } }

    private bool forceProjectileSpread = false;

    public bool ForceProjectileSpread { get => forceProjectileSpread; set { forceProjectileSpread = value; } }

    private int extraManaKillAmount = 0;

    public int ExtraManaKillAmount { get => extraManaKillAmount; set { extraManaKillAmount = value; } }

    private int amountOfExtraCasts = 0;

    public int AmountOfExtraCasts { get => amountOfExtraCasts; set { amountOfExtraCasts = value; } }



    [Header("Other")]

    private float expGainPercentage = 0.0f;
    public float ExpGainPercentage { get { return expGainPercentage; } set { expGainPercentage = value; } }

    private float expDoubleChance= 0.0f;

    public float ExpDoubleChance { get => expDoubleChance; set { expDoubleChance = value; } }

    private float goldPercentage = 0.0f;
    public float GoldPercentage { get { return goldPercentage; } set { goldPercentage = value; } }


    private int goldAmount;

    public int GoldAmount { get { return goldAmount; } set { goldAmount = value; } }

    [SerializeField] private int currentLevel = 1;

    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }

    private int nextLevelUpExperience = 0;

    public int NextLevelUpExperience { get { return nextLevelUpExperience; } }

    [SerializeField] private int currentExperience = 0;

    public int CurrentExperience { get { return currentExperience; } set { currentExperience = value; } }

    public float ExperiencePercent;


    // Events
    public UnityEvent<int> OnLevelUp = new();
    public UnityEvent<int> OnGoldChange = new();
    public UnityEvent<float> OnExpChanged = new();
    public UnityEvent<ItemData> OnItemAdded = new();
    public UnityEvent OnSpellShrineUpgrade = new();
    public UnityEvent OnCrit = new();
    public UnityEvent OnStatUpdate = new();

    private enum CharacterClass { None, Warrior,Thief,Mage}

    [SerializeField] private LevelBuffBattleLoot characterBuffBattleLoot;
    public LevelBuffBattleLoot CharacterBuffBattleLoot => characterBuffBattleLoot;

    private List<BuffBattleLoot> buffBattleLoots = new List<BuffBattleLoot>();
    public List<BuffBattleLoot> BuffBattleLoots => buffBattleLoots;

    public List<BuffBattleLoot> tempBuffBattleLoot = new List<BuffBattleLoot>();

    public List<BuffBattleLoot> TempBuffBattleLoot => tempBuffBattleLoot;


    public void AddGold(int amount)
    {
        goldAmount += amount + Mathf.CeilToInt((float)amount * goldPercentage);
        OnGoldChange.Invoke(goldAmount);
    }

    public void RemoveGold(int amount)
    {
        goldAmount -= amount + Mathf.CeilToInt((float)amount * goldPercentage);
        OnGoldChange.Invoke(goldAmount);
    }

    public void AddExperience(int experience)
    {
     
        int amountOfGainExperience = experience + Mathf.CeilToInt((float)experience * expGainPercentage);
        float doubleingChance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (doubleingChance < expDoubleChance)
        {
            amountOfGainExperience += amountOfGainExperience;
        }


        currentExperience += amountOfGainExperience;
        if (currentExperience >= nextLevelUpExperience)
        {
            currentLevel++;
            if(CharacterBuffBattleLoot != null)
            {
                CharacterBuffBattleLoot.UpdateBuffBattleLoot(currentPlayer, this);
            }
            nextLevelUpExperience = ExpForNextLevel(currentLevel);
            int experienceDifference = currentExperience - nextLevelUpExperience;
            currentExperience = 0;
            if (experienceDifference > 0) {
                AddExperience(experienceDifference);
            }

            OnLevelUp.Invoke(currentLevel);
        }
        ExperiencePercent = (float)currentExperience / (float)nextLevelUpExperience;
        OnExpChanged.Invoke(ExperiencePercent);
    }

    private int ExpForNextLevel(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(level, 1.1f));
    }

    public float GetCritModifier()
    {
        float critPercentage = 0f;
        if (critPercentage < Random.Range(0.0f, 1.0f)) {
            return 2 + extraCritDamage;
            OnCrit.Invoke();
        }

        return 1;
    }

    public void AddBuffBattleLoot(BuffBattleLoot buff)
    {
        BuffBattleLoot copiedBuff = Instantiate(buff);
        BuffBattleLoots.Add(copiedBuff);
        copiedBuff.BuffBattleLootAdded(currentPlayer, this);
        OnStatUpdate.Invoke();
    }

    public void AddTempBuffBattleLoot(BuffBattleLoot buff)
    {
        BuffBattleLoot copiedBuff = Instantiate(buff);
        TempBuffBattleLoot.Add(copiedBuff);
        copiedBuff.BuffBattleLootAdded(currentPlayer, this);
        copiedBuff.StartTempBuff();
        OnStatUpdate?.Invoke();
    }

    private void RemoveTempBuffBattleLoot(BuffBattleLoot buff)
    {
        TempBuffBattleLoot.Remove(buff);
        buff.BuffBattleLootRemoved(currentPlayer, this);
        OnStatUpdate?.Invoke();
    }


    public bool ForceUpdateStats = false;

    [SerializeField] private WeaponData currentEquipedWeapon;
    public WeaponData CurrentEquipedWeapon
    {
        get => currentEquipedWeapon;
        set => currentEquipedWeapon = value;
    }

    [SerializeField] private MagicSpell currentMagicSpell_1;
    public MagicSpell CurrentMagicSpell_1
    {
        get => currentMagicSpell_1;
        set => currentMagicSpell_1 = value;
    }

    [SerializeField] private MagicSpell currentMagicSpell_2;
    public MagicSpell CurrentMagicSpell_2
    {
        get => currentMagicSpell_2;
        set => currentMagicSpell_2 = value;
    }

    [SerializeField] private MagicSpell currentMagicSpell_3;
    public MagicSpell CurrentMagicSpell_3
    {
        get => currentMagicSpell_3;
        set => currentMagicSpell_3 = value;
    }

    [SerializeField] private List<WeaponData> weaponInventory = new();
    public List<WeaponData > WeaponInventory => weaponInventory;

    [SerializeField] private List<ConsumbaleData> consumableInventory = new();
    public List<ConsumbaleData> ConsumableInventory => consumableInventory;

    [SerializeField] private List<MagicSpell> magicSpellInventory = new();
    public List<MagicSpell> MagicSpellInventory => magicSpellInventory;


    [SerializeField] private ConsumbaleData consumable_1;
    public ConsumbaleData Consumable_1
    {
        get { return consumable_1; }
        set { consumable_1 = value; }
    }

    [SerializeField] private ConsumbaleData consumable_2;
    public ConsumbaleData Consumable_2
    {
        get { return consumable_2; }
        set { consumable_2 = value; }
    }

    [SerializeField] private ConsumbaleData consumable_3;
    public ConsumbaleData Consumable_3
    {
        get { return consumable_3; }
        set { consumable_3 = value; }
    }

    private GameObject currentPlayer;

    public void Init(GameObject player)
    {
        currentPlayer = player;

        List<ConsumbaleData> copiedConsumables = new();
        foreach(ConsumbaleData data in consumableInventory)
        {
            copiedConsumables.Add(Instantiate(data));
        }
        consumableInventory = copiedConsumables;

        List<WeaponData> copiedWeapon = new List<WeaponData>();
        foreach(WeaponData data in weaponInventory)
        {
            copiedWeapon.Add(Instantiate(data));
        }
        weaponInventory = copiedWeapon;

        List<MagicSpell> copiedMagicSpells = new List<MagicSpell>();
        foreach (MagicSpell data in magicSpellInventory)
        {
            copiedMagicSpells.Add(Instantiate(data));
        }
        magicSpellInventory = copiedMagicSpells;


        if (Consumable_1 != null)
        {
            consumable_1 = Instantiate(consumable_1);
        }
        if (Consumable_2 != null)
        {
            consumable_2 = Instantiate(consumable_2);
        }
        if (Consumable_3 != null)
        {
            consumable_3 = Instantiate(consumable_3);
        }
        nextLevelUpExperience = ExpForNextLevel(currentLevel);
        if (characterBuffBattleLoot != null) { 
            characterBuffBattleLoot.BuffBattleLootAdded(currentPlayer, this);
        }
    }

    public void GameUpdate()
    {
        if(tempBuffBattleLoot.Count > 0)
        {
            BuffBattleLoot toRemoveBuff = null;
            foreach(BuffBattleLoot buffBattleLoot in tempBuffBattleLoot)
            {
                buffBattleLoot.CurrentTemporaryBuffTime -= Time.deltaTime;
                if (!buffBattleLoot.IsTempBuffActiv)
                {
                    toRemoveBuff = buffBattleLoot;
                }
            }

            if(toRemoveBuff != null)
            {
                RemoveTempBuffBattleLoot(toRemoveBuff);
            }
        }
    }

    public void ForceLevelUp()
    {
        AddExperience(nextLevelUpExperience);
    }

    public void ReadPlayerDataStats(PlayerData playerData)
    {
   
    }

    public void UseItem1()
    {
        UseItem(consumable_1);
        if (consumable_1.Amount <= 0)
        {
            consumable_1 = null;
        }
    }

    public void UseItem2()
    {
        UseItem(consumable_2);
        if (consumable_2.Amount <= 0)
        {
            consumable_2 = null;
        }
    }

    public void UseItem3()
    {
        UseItem(consumable_3);
        if (consumable_3.Amount <= 0)
        {
            consumable_3 = null;
        }
    }


    public void UseItem(ConsumbaleData itemSlot)
    {
        itemSlot.Use(currentPlayer);
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData is ConsumbaleData consumbale)
        {
            ConsumbaleData foundConsumable = FindConsumable(consumbale.GUID);
            if (foundConsumable != null)
            {
                foundConsumable.AddConsumable(foundConsumable.Amount);
            }
            else
            {
                ConsumableInventory.Add(consumbale);
            }
        }
        else if (itemData is WeaponData weaponData)
        {
            weaponInventory.Add(weaponData);
        }
        else if (itemData is GoldItem goldItem) {
            AddGold(goldItem.GoldAmount);
        }
        else if(itemData is MagicSpell magicSpell)
        {
            magicSpellInventory.Add(magicSpell);
        }
        OnItemAdded.Invoke(itemData);
    }

    private ConsumbaleData FindConsumable(string GUID)
    {
        foreach(ConsumbaleData consumbaleData in consumableInventory)
        {
            if(consumbaleData.GUID == GUID)
            {
                return consumbaleData;
            }
        }
        return null;
    }


    public static int CalculateMeleeChargeDamage(float minDamage,float maxDamage,float chargeAmount,int strengthStat)
    {       
        return Mathf.RoundToInt( (Mathf.Lerp(minDamage, maxDamage, chargeAmount) + Mathf.Pow(strengthStat, 0.8f)));
    }

    public static int CalculateChargeDamage(float minDamage, float maxDamage, float chargeAmount)
    {
        return Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, chargeAmount)); ;
    }

    internal void RegSpellMana(int regAmount)
    {
        for(int i = 0; i< regAmount; i++)
        {
            RegSpellMana();
        }
    }

    internal void RegSpellMana()
    {
        if(CurrentMagicSpell_1 != null)
        {
            CurrentMagicSpell_1.RegMana();
        }
        if (CurrentMagicSpell_2 != null)
        {
            CurrentMagicSpell_2.RegMana();
        }
        if (CurrentMagicSpell_3 != null)
        {
            CurrentMagicSpell_3.RegMana();
        }
    }

    internal bool HasEnoughGold(int price)
    {
        return goldAmount >= price;
    }
}

