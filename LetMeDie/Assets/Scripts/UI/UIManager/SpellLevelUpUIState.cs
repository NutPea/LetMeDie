using Essentials;
using Mono.Cecil;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SpellLevelUpUIState : UIStateComponent
{
    [SerializeField] private BattleLootTable battleLootTable;

    private PlayerStatHandler playerStatHandler;
    private PlayerWeaponEquiper playerWeaponEquiper;
    private PlayerData playerData;

    [SerializeField] private BattleLootButton battleLootButton1;
    [SerializeField] private BattleLootButton battleLootButton2;
    [SerializeField] private BattleLootButton battleLootButton3;

    [Header("WaveRaritys")]
    [SerializeField] private int uncommonLevelAmount = 5;
    [SerializeField] private int rareLevelAmount = 10;
    [SerializeField] private int epicLevelAmount = 15;
    [SerializeField] private int legendaryLevelAmount = 20;


    [Header("Transition")]
    [SerializeField] private GameObject levelUpUI;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeOutQuad;

    private bool canChooseSomething = false;


    public override void OnInitUIState()
    {
        base.OnInitUIState();
        battleLootButton1.OnChooseBattle.AddListener(SetLoot);
        battleLootButton2.OnChooseBattle.AddListener(SetLoot);
        battleLootButton3.OnChooseBattle.AddListener(SetLoot);
        playerStatHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();
        playerWeaponEquiper = playerStatHandler.GetComponent<PlayerWeaponEquiper>();
        playerData = playerStatHandler.PlayerData;
        battleLootTable.Init();
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        OnSpellLevelUp(playerData.CurrentLevel);
        canChooseSomething = false;
        TransitionIn();
    }

    private void TransitionIn()
    {
        levelUpUI.transform.localScale = Vector3.zero;
        LeanTween.scale(levelUpUI, Vector3.one, transitionTime).setEase(tweenType).setOnComplete(Unlock).setIgnoreTimeScale(true);
    }

    private void Unlock()
    {
        canChooseSomething = true;
    }
    private void SetLoot(BattleLoot battleLoot)
    {
        if (!canChooseSomething)
        {
            return;
        }


        if (battleLoot is WeaponBattleLoot weaponBattleLoot)
        {
            playerData.AddItem(weaponBattleLoot.WeaponData);
            if(weaponBattleLoot.WeaponData is MagicSpell magic)
            {
                if(playerData.CurrentMagicSpell_1 == null)
                {
                    playerWeaponEquiper.EquipSpell1(magic);
                }
                else if(playerData.CurrentMagicSpell_2 == null)
                {
                    playerWeaponEquiper.EquipSpell2(magic);
                }
                else if(playerData.CurrentMagicSpell_3 == null)
                {
                    playerWeaponEquiper.EquipSpell3(magic);
                }
                else
                {
                    Debug.LogError("Something went wrong!");
                }
            }
        }
        if(battleLoot is SpellInflluenceBattleLoot spellInflluenceBattleLoot)
        {
            spellInflluenceBattleLoot.UpgradeSpell();
        }
        else if (battleLoot is BuffBattleLoot buffBattleLoot) {
        
        
        }

        battleLootTable.ChooseBattleLoot(battleLoot);
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
        playerData.OnSpellShrineUpgrade.Invoke();
    }

    private void OnSpellLevelUp(int index)
    {
        float commonPercentage = 0.0f;
        float uncommonPercentage = 0.0f;
        float rarePercentage = 0.0f;
        float epicPercentage = 0.0f;
        float legendaryPercentage = 0.0f;

        (commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage) = GetDropPercentage(index);

        int possibleTrys = 100;

        BattleLoot battleLoot1 = null;
        List<BattleLoot> currentAvailableBattleLoots = new();
        battleLootTable.AvailableLoots.ForEach(battleLoot => { currentAvailableBattleLoots.Add(battleLoot); });
        if (playerData.CurrentMagicSpell_1 != null) {

            SpellInflluenceBattleLoot spellInflluenceBattleLoot = ScriptableObject.CreateInstance<SpellInflluenceBattleLoot>();
            spellInflluenceBattleLoot.SetSpell(playerData.CurrentMagicSpell_1, GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
            battleLoot1 = spellInflluenceBattleLoot;
        }
        else
        {
            battleLoot1 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count-1)];
            currentAvailableBattleLoots.Remove(battleLoot1);
        }

        BattleLoot battleLoot2 = null;
        if (playerData.CurrentMagicSpell_2 != null)
        {
            SpellInflluenceBattleLoot spellInflluenceBattleLoot = ScriptableObject.CreateInstance<SpellInflluenceBattleLoot>();
            spellInflluenceBattleLoot.SetSpell(playerData.CurrentMagicSpell_2, GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
            battleLoot2 = spellInflluenceBattleLoot;

        }
        else
        {
            battleLoot2 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count-1)];
            currentAvailableBattleLoots.Remove(battleLoot2);
        }

        BattleLoot battleLoot3 = null;
        if (playerData.CurrentMagicSpell_3 != null)
        {
            SpellInflluenceBattleLoot spellInflluenceBattleLoot = ScriptableObject.CreateInstance<SpellInflluenceBattleLoot>();
            spellInflluenceBattleLoot.SetSpell(playerData.CurrentMagicSpell_3, GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
            battleLoot3 = spellInflluenceBattleLoot;

        }
        else
        {
            battleLoot3 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count-1)];
            currentAvailableBattleLoots.Remove(battleLoot3);
        }

        battleLootButton1.SetBattleLoot(battleLoot1);
        battleLootButton2.SetBattleLoot(battleLoot2);
        battleLootButton3.SetBattleLoot(battleLoot3);
        
    }


    public static BattleLoot.LootRarity GetRarity(float commonPercentage,float uncommonPercentage,float rarePercentage,float epicPercentage,float legendaryPercentage)
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
        if(waveIndex >= legendaryLevelAmount)
        {
            return (0.05f, 0.10f, 0.20f, 0.4f, 0.25f);
        }
        else if(waveIndex >= epicLevelAmount)
        {
            return (0.19f, 0.3f, 0.35f, 0.1f, 0.01f);
        }
        else if (waveIndex >= rareLevelAmount)
        {
            return (0.55f, 0.3f, 0.15f, 0, 0);
        }
        else if (waveIndex >= uncommonLevelAmount)
        {
            return (0.75f, 0.25f, 0, 0, 0);
        }
        else {
            return (1, 0, 0, 0, 0);
        }


        return (0, 0, 0, 0, 0);
    }

    public static (float, float, float, float, float) GetDropPercentage(BattleLoot.LootRarity rarity)
    {
        switch (rarity)
        {
            case BattleLoot.LootRarity.Common: return (1, 0, 0, 0, 0);
            case BattleLoot.LootRarity.Uncommen: return (0.75f, 0.25f, 0, 0, 0);
            case BattleLoot.LootRarity.Rare: return (0.55f, 0.3f, 0.15f, 0, 0);
            case BattleLoot.LootRarity.Epic: return (0.19f, 0.3f, 0.35f, 0.1f, 0.01f);
            case BattleLoot.LootRarity.Legendary: return (0.05f, 0.10f, 0.20f, 0.4f, 0.25f);
        }
        return (0, 0, 0, 0, 0);
    }



}
