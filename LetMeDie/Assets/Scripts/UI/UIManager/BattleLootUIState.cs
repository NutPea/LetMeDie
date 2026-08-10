using Essentials;
using System;
using System.Collections;
using UnityEngine;

public class BattleLootUIState : UIStateComponent
{
    [SerializeField] private WaveHandler waveHandler;
    [SerializeField] private BattleLootTable battleLootTable;

    private PlayerStatHandler playerStatHandler;
    private PlayerWeaponEquiper playerWeaponEquiper;
    private PlayerData playerData;

    [SerializeField] private BattleLootButton battleLootButton1;
    [SerializeField] private BattleLootButton battleLootButton2;
    [SerializeField] private BattleLootButton battleLootButton3;

    [Header("WaveRaritys")]
    [SerializeField] private int uncommonWaveAmount = 5;
    [SerializeField] private int rareWaveAmount = 10;
    [SerializeField] private int epicWaveAmount = 15;
    [SerializeField] private int legendaryWaveAmount = 20;



    public override void OnInitUIState()
    {
        base.OnInitUIState();
        waveHandler.OnWaveChange.AddListener(OnWaveChange);
        battleLootButton1.OnChooseBattle.AddListener(SetLoot);
        battleLootButton2.OnChooseBattle.AddListener(SetLoot);
        battleLootButton3.OnChooseBattle.AddListener(SetLoot);
        playerStatHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();
        playerWeaponEquiper = playerStatHandler.GetComponent<PlayerWeaponEquiper>();
        playerData = playerStatHandler.PlayerData;
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
    }

    private void SetLoot(BattleLoot battleLoot)
    {
        if (battleLoot is WeaponBattleLoot weaponBattleLoot)
        {
            playerData.AddItem(weaponBattleLoot.WeaponData);
            if(weaponBattleLoot.WeaponData is MagicSpell magic)
            {

            }
            else
            {
                playerWeaponEquiper.EquipWeapon(weaponBattleLoot.WeaponData);
            }
        }
        else if (battleLoot is ConsumableBattleLoot consumableBattleLoot)
        {


        }
        else if (battleLoot is BuffBattleLoot buffBattleLoot) {
        
        
        }


        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    private void OnWaveChange(int index, Wave wave)
    {
        if (wave.waveType != Wave.WaveType.Loot) {
            return;
        }
        SUIManager.Instance.ChangeToUIState("BattleLoot");
        float commonPercentage = 0.0f;
        float uncommonPercentage = 0.0f;
        float rarePercentage = 0.0f;
        float epicPercentage = 0.0f;
        float legendaryPercentage = 0.0f;

        (commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage) = GetDropPercentage(index);

        int possibleTrys = 100;
        BattleLoot battleLoot1 = battleLootTable.GetRandomLoot(GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
        BattleLoot battleLoot2 = battleLootTable.GetRandomLoot(GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage)); ;
        BattleLoot battleLoot3 = battleLootTable.GetRandomLoot(GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage)); ;
        for (int i = 0; i < possibleTrys; i++) {
            if(battleLoot2 != battleLoot1){
                break;
            }
            battleLoot2 = battleLootTable.GetRandomLoot(GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
        }

        for (int i = 0; i < possibleTrys; i++)
        {
            if (battleLoot3 != battleLoot1 && battleLoot3 != battleLoot2){
                break;
            }
            battleLoot3 = battleLootTable.GetRandomLoot(GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
        }

        battleLootButton1.SetBattleLoot(battleLoot1);
        battleLootButton2.SetBattleLoot(battleLoot2);
        battleLootButton3.SetBattleLoot(battleLoot3);
    }


    private BattleLoot.LootRarity GetRarity(float commonPercentage,float uncommonPercentage,float rarePercentage,float epicPercentage,float legendaryPercentage)
    {
        float randomValue = UnityEngine.Random.Range(0.0f,1.0f);
        if (randomValue <= commonPercentage)
        {
            return BattleLoot.LootRarity.Common;
        }
        else if (randomValue <= commonPercentage+uncommonPercentage)
        {
            return BattleLoot.LootRarity.Uncommen;
        }
        else if (randomValue <= commonPercentage+uncommonPercentage+rarePercentage)
        {
            return BattleLoot.LootRarity.Rare;
        }
        else if (randomValue < commonPercentage+uncommonPercentage+rarePercentage+epicPercentage)
        {
            return BattleLoot.LootRarity.Epic;
        }
        else
        {
            return BattleLoot.LootRarity.Legendary;
        }


    }

    private (float,float,float,float,float) GetDropPercentage(int waveIndex)
    {
        if(waveIndex >= legendaryWaveAmount)
        {
            return (0.05f, 0.10f, 0.20f, 0.4f, 0.25f);
        }
        else if(waveIndex >= epicWaveAmount)
        {
            return (0.19f, 0.3f, 0.35f, 0.1f, 0.01f);
        }
        else if (waveIndex >= rareWaveAmount)
        {
            return (0.55f, 0.3f, 0.15f, 0, 0);
        }
        else if (waveIndex >= uncommonWaveAmount)
        {
            return (0.75f, 0.25f, 0, 0, 0);
        }
        else {
            return (1, 0, 0, 0, 0);
        }


        return (0, 0, 0, 0, 0);
    }


}
