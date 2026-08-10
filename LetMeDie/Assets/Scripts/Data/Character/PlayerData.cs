using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "Character/PlayerData", order = 1)]
public class PlayerData : HealthData
{
    public override int Health => baseHealth + CalculateHealth();

    // Events
    public UnityEvent<int> OnLevelUp = new();
    public UnityEvent<float> OnExpChanged = new();
    public UnityEvent<ItemData> OnItemAdded = new();

    [SerializeField] private int baseMana = 10;

    public int Mana { get { return baseMana + CalculateMana(); }}


    [SerializeField] private int characterLevel = 1;
    public int CharacterLevel {  get { return characterLevel; } set { characterLevel = value; } }

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

    private enum CharacterClass { None, Warrior,Thief,Mage}

    public void AddExperience(int experience)
    {
        currentExperience += experience;
        int levelUpAmount = currentLevel >= LevelUpTable.Length-1 ? LevelUpTable[LevelUpTable.Length-1] : LevelUpTable[characterLevel];
        if (currentExperience >= nextLevelUpExperience)
        {
            currentLevel++;
            nextLevelUpExperience = currentLevel >= LevelUpTable.Length - 1 ? LevelUpTable[LevelUpTable.Length - 1] : LevelUpTable[characterLevel];
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


    [SerializeField] private int strength;

    // Melee Damage and Health 
    public int Strength {
        get { return strength; } set { strength = value; }
    }
    [SerializeField] private int dexterity;

    //JumpHeight & Aircontroll  
    public int Dexterity { get { return dexterity; } set { dexterity = value; } }

    [SerializeField] private int intelligence;

    //Max Mana 
    public int Intelligence { get => intelligence; set => intelligence = value; }
    [SerializeField] private int resilience;

    // Health and Status Resistance
    public int Resilience { get => resilience; set => resilience = value; }
    [SerializeField] private int speed;

    // RunningSpeed
    public int Speed { get => speed; set => speed = value; }

    // Increases Chance for rare loot and gold. Have a chance to avoid damage all together and hit a critical strike

    [SerializeField] private int luck;
    
    public int Luck { get => luck; set => luck = value; }

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

    [SerializeField] private GoldItem _goldItem;

    public GoldItem GoldItem => _goldItem;

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
        _goldItem = Instantiate(_goldItem);
    }

    public void LevelUp()
    {
        characterLevel = currentLevel;
    }

    public void ReadPlayerDataStats(PlayerData playerData)
    {
        strength = playerData.strength;
        dexterity = playerData.dexterity;
        intelligence = playerData.intelligence;
        speed = playerData.speed;
        resilience = playerData.resilience;
        luck = playerData.luck;
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
            _goldItem.AddGold(goldItem.GoldAmount);
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

    private int CalculateMana()
    {
        return Mathf.RoundToInt( 3 * intelligence );
    }

    private int CalculateHealth()
    {
       return Mathf.RoundToInt(Mathf.Pow(strength, 1.5f) + 3 * resilience);
    }

    public static float CalculateMovementSpeed(float baseMovementSpeed, float speedStat)
    {
        return (baseMovementSpeed + 2 * Mathf.Pow(speedStat, 0.8f));
    }

    public static float CalculateJumpForce(float baseJumpHeight, float dexterityStat)
    {
        return (baseJumpHeight + 0.1f * Mathf.Pow(dexterityStat, 0.8f));
    }

    public static float CalculateAirMovementSpeed(float baseAirMovementSpeed, int dexterityStat)
    {
        return (baseAirMovementSpeed + dexterityStat / 10);
    }

    public static float CalculateMelleDamage(int strength)
    {
        return Mathf.Pow(strength, 0.8f);
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

