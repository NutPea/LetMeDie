using Essentials;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffItemGetUIState : UIStateComponent
{

   // [SerializeField] private TextMeshPro rarity;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image rarityImage;
    [SerializeField] private Image image;
    [SerializeField] private BattleLootTable buffItemTable;

    private BuffBattleLoot choosenBuffBattleLoot;
    private BattleLoot.LootRarity choosenRarity;
    [SerializeField] private Button accept;
    [SerializeField] private Button ban;

    PlayerData playerData;


    [Header("Transition")]
    [SerializeField] private GameObject levelUpUI;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeOutQuad;

    private bool canChooseSomething = false;

    private List<BattleLoot> common = new();
    private List<BattleLoot> uncommon = new();
    private List<BattleLoot> rare = new();
    private List<BattleLoot> epic = new();
    private List<BattleLoot> legendary = new();

    [Header("Debug")]
    [SerializeField] private Sprite backupItemImage;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        buffItemTable.Init();
        accept.onClick.AddListener(Accept);
        ban.onClick.AddListener(Ban);

        playerData = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData;

        buffItemTable.FillRarityTables(common,uncommon,rare,epic,legendary);
    }

    private void Ban()
    {
        switch (choosenRarity)
        {
            case BattleLoot.LootRarity.Common: common.Remove(choosenBuffBattleLoot); break;
            case BattleLoot.LootRarity.Uncommen: uncommon.Remove(choosenBuffBattleLoot); break;
            case BattleLoot.LootRarity.Rare: rare.Remove(choosenBuffBattleLoot); break;
            case BattleLoot.LootRarity.Epic: epic.Remove(choosenBuffBattleLoot); break;
            case BattleLoot.LootRarity.Legendary: legendary.Remove(choosenBuffBattleLoot); break;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    private void Accept()
    {
        playerData.AddBuffBattleLoot(choosenBuffBattleLoot);
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        choosenBuffBattleLoot = GetBuffBattleLoot();
        name.text = choosenBuffBattleLoot.Name;
        description.text = choosenBuffBattleLoot.Description;
        if(choosenBuffBattleLoot.Icon == null)
        {
            image.sprite = backupItemImage;
        }
        else
        {
            image.sprite = choosenBuffBattleLoot.Icon;
        }

        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        Time.timeScale = 0.0f;
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

    public override void OnExitUIState()
    {
        base.OnExitUIState();
        Time.timeScale = 1.0f;
    }
    public BuffBattleLoot GetBuffBattleLoot()
    {
        choosenRarity = BattleLootTable.GetRarity(playerData.Luck);
        rarityImage.color = SGameManager.Instance.GetRarityColor(choosenRarity);

        switch (choosenRarity)
        {
            case BattleLoot.LootRarity.Common: return common[Random.Range(0, common.Count - 1)] as BuffBattleLoot; 
            case BattleLoot.LootRarity.Uncommen: return uncommon[Random.Range(0, uncommon.Count - 1)] as BuffBattleLoot; 
            case BattleLoot.LootRarity.Rare: return rare[Random.Range(0, rare.Count - 1)] as BuffBattleLoot; 
            case BattleLoot.LootRarity.Epic: return epic[Random.Range(0, epic.Count - 1)] as BuffBattleLoot;
            case BattleLoot.LootRarity.Legendary: return legendary[Random.Range(0, legendary.Count - 1)] as BuffBattleLoot; 
        }


        return buffItemTable.AvailableLoots[Random.Range(0,buffItemTable.AvailableLoots.Count -1)] as BuffBattleLoot;
    }

}
