using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using static PixelCrushers.AnimatorSaver;

[CreateAssetMenu(fileName = "Data", menuName = "Character/PlayerData", order = 1)]
public class PlayerData : HealthData
{



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

    private int healthRegAmount = 0;
    public int HealthRegAmount { get { return healthRegAmount; } set { healthRegAmount = value; } }

    private float healthRegRate = 0;
    public float HealthRegRate { get { return healthRegRate; } set { healthRegRate = value; } }

    private float weaponBaseDamagePercentage = 0.0f;
    public float WeaponBaseDamagePercentage { get { return weaponBaseDamagePercentage; } set { weaponBaseDamagePercentage = value; } }

    private float spellBaseDamagePercentage = 0.0f;
    public float SpellBaseDamagePercentage { get { return spellBaseDamagePercentage; } set { spellBaseDamagePercentage = value; } }

    [Header("Other")]

    private float expGainPercentage = 0.0f;
    public float ExpGainPercentage { get { return expGainPercentage; } set { expGainPercentage = value; } }

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

    private int[] LevelUpTable = {
        300,900,1200,2000
    
    };

    // Events
    public UnityEvent<int> OnLevelUp = new();
    public UnityEvent<float> OnExpChanged = new();
    public UnityEvent<ItemData> OnItemAdded = new();
    public UnityEvent OnStatUpdate = new();

    private enum CharacterClass { None, Warrior,Thief,Mage}

    private List<BuffBattleLoot> buffBattleLoots = new List<BuffBattleLoot>();
    public List<BuffBattleLoot> BuffBattleLoots => buffBattleLoots;

    public void AddGold(int amount)
    {
        goldAmount += amount + Mathf.CeilToInt((float)amount * goldPercentage);
    }

    public void AddExperience(int experience)
    {
        currentExperience += experience + Mathf.CeilToInt((float)experience * expGainPercentage);
        int levelUpAmount = currentLevel >= LevelUpTable.Length-1 ? LevelUpTable[LevelUpTable.Length-1] : LevelUpTable[currentLevel];
        if (currentExperience >= nextLevelUpExperience)
        {
            currentLevel++;
            nextLevelUpExperience = currentLevel >= LevelUpTable.Length - 1 ? LevelUpTable[LevelUpTable.Length - 1] : LevelUpTable[currentLevel];
            int experienceDifference = currentExperience - nextLevelUpExperience;
            if (experienceDifference <= 0) {
                currentExperience = 0;
            }
            else {
                AddExperience(experienceDifference);
            }
            OnLevelUp.Invoke(currentLevel);
        }
        ExperiencePercent = (float)currentExperience / (float)nextLevelUpExperience;
        OnExpChanged.Invoke(ExperiencePercent);
    }



    public void AddBuffBattleLoot(BuffBattleLoot buff)
    {
        BuffBattleLoot copiedBuff = Instantiate(buff);
        BuffBattleLoots.Add(buff);
        buff.BuffBattleLootAdded(currentPlayer, this);
        OnStatUpdate.Invoke();
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

}

