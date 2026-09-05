using Essentials;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIState : UIStateComponent
{

    [SerializeField] private BattleLootTable battleLootTable;

    private PlayerStatHandler playerStatHandler;
    private PlayerWeaponEquiper playerWeaponEquiper;
    private PlayerData playerData;

    [SerializeField] private BattleLootButton battleLootButton1;
    [SerializeField] private BattleLootButton battleLootButton2;
    [SerializeField] private BattleLootButton battleLootButton3;



    [Header("Transition")]
    [SerializeField] private GameObject levelUpUI;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private LeanTweenType tweenType;

    private bool canChooseSomething = false;


    public override void OnInitUIState()
    {
        base.OnInitUIState();
        battleLootButton1.OnChooseBattle.AddListener(SetLoot);
        battleLootButton2.OnChooseBattle.AddListener(SetLoot);
        battleLootButton3.OnChooseBattle.AddListener(SetLoot);
        playerStatHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();
        playerStatHandler.PlayerData.OnLevelUp.AddListener(OnLevelUp);
        playerWeaponEquiper = playerStatHandler.GetComponent<PlayerWeaponEquiper>();
        playerData = playerStatHandler.PlayerData;
        battleLootTable.Init();
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
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
            if (weaponBattleLoot.WeaponData is MagicSpell magic)
            {
                if (playerData.CurrentMagicSpell_1 == null)
                {
                    playerWeaponEquiper.EquipSpell1(magic);
                }
                else if (playerData.CurrentMagicSpell_2 == null)
                {
                    playerWeaponEquiper.EquipSpell2(magic);
                }
                else if (playerData.CurrentMagicSpell_3 == null)
                {
                    playerWeaponEquiper.EquipSpell3(magic);
                }
                else
                {
                    Debug.LogError("Something went wrong!");
                }
            }
        }
        if (battleLoot is SpellInflluenceBattleLoot spellInflluenceBattleLoot)
        {
            spellInflluenceBattleLoot.UpgradeSpell();
        }
        else if (battleLoot is BuffBattleLoot buffBattleLoot)
        {
            playerData.AddBuffBattleLoot(buffBattleLoot);
        }

        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    private void OnLevelUp(int index)
    {
        SUIManager.Instance.ChangeToUIState("LevelUp");

        BattleLoot.LootRarity battleLoot1Rarity = BattleLootTable.GetRarity(playerData.Luck);
        BattleLoot.LootRarity battleLoot2Rarity = BattleLootTable.GetRarity(playerData.Luck);
        BattleLoot.LootRarity battleLoot3Rarity = BattleLootTable.GetRarity(playerData.Luck);


        List<BattleLoot> currentAvailableBattleLoots = new();
        battleLootTable.AvailableLoots.ForEach(battleLoot => { currentAvailableBattleLoots.Add(battleLoot); });
      
        BattleLoot battleLoot1 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count - 1)];
        currentAvailableBattleLoots.Remove(battleLoot1);
        BattleLoot battleLoot2 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count - 1)];
        currentAvailableBattleLoots.Remove(battleLoot2);
        BattleLoot battleLoot3 = currentAvailableBattleLoots[UnityEngine.Random.Range(0, currentAvailableBattleLoots.Count - 1)];
       currentAvailableBattleLoots.Remove(battleLoot3);

        battleLoot1.CalculateValues();
        battleLoot2.CalculateValues();
        battleLoot3.CalculateValues();

        battleLoot1.lootRarity = battleLoot1Rarity;
        battleLoot2.lootRarity = battleLoot2Rarity;
        battleLoot3.lootRarity = battleLoot3Rarity;


        battleLootButton1.SetBattleLoot(battleLoot1);
        battleLootButton2.SetBattleLoot(battleLoot2);
        battleLootButton3.SetBattleLoot(battleLoot3);

    }




}
