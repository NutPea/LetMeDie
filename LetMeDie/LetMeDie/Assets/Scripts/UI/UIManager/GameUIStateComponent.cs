using Essentials;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIStateComponent : UIStateComponent
{
    private PlayerResourceHandler playerResourceHandler;
    private PlayerCombatController playerCombatController;
    private PlayerStatHandler playerStatHandler;
    private PlayerAnimationController playerAnimationController;
    private PlayerInteractionHandler playerInteractionHandler;
    private PlayerPickUpHandler playerPickUpHandler;

    private PlayerData PlayerData => playerStatHandler.PlayerData;

    [SerializeField] private CrossHairUIHandler crossAirUI;

    [Header("Item")]
    [SerializeField] private ItemUIHandler itemUIHandler_1;

    [SerializeField] private ItemUIHandler itemUIHandler_2;

    [SerializeField] private ItemUIHandler itemUIHandler_3;

    [Header("Spell")]

    [SerializeField] private ItemUIHandler spellHandler_1;

    [SerializeField] private ItemUIHandler spellHandler_2;

    [SerializeField] private TextMeshProUGUI itemAccuiredText;
    [SerializeField] private float accuiredTextShowTime = 1.5f; 

    [Header("Bar")]
    [SerializeField] private BarUIHandler healthBarHandler;
    [SerializeField] private BarUIHandler manaBarHandler;
   // [SerializeField] private BarUIHandler expBarHandler;

    [Header("Borders")]
    [SerializeField] private LeanTweenType borderType;
    [SerializeField] private Image levelUpBorder;
    [SerializeField] private float levelUpBorderTime;

    [SerializeField] private Image damageBorder;
    [SerializeField] private float damageBorderTime;

    [SerializeField] private Image healBorder;
    [SerializeField] private float healBorderTime;


    public override void OnInitUIState()
    {
        base.OnInitUIState();

        GameObject player = PlayerSingelton.instance.gameObject;
        playerResourceHandler = player.GetComponent<PlayerResourceHandler>();
        playerResourceHandler.OnDamaged.AddListener(GetDamage);
        playerResourceHandler.OnHealthUpdate.AddListener(UpdateHealth);
        playerResourceHandler.OnHeal.AddListener(ShowHeal);

        playerCombatController = player.GetComponent<PlayerCombatController>();
        playerCombatController.OnCharge.AddListener(SetChargeValue);
        playerStatHandler = player.GetComponent<PlayerStatHandler>();
        playerAnimationController = player.GetComponent<PlayerAnimationController>();

        playerInteractionHandler = player.GetComponent<PlayerInteractionHandler>();
        playerPickUpHandler = player.GetComponent<PlayerPickUpHandler>();

        playerCombatController.OnEndCharge.AddListener(EndChargeValue);

        healthBarHandler.SetValue(playerResourceHandler.currentHealth, playerResourceHandler.healthData.Health);
        manaBarHandler.SetValue(playerResourceHandler.CurrentMana, playerStatHandler.PlayerData.Mana);
       // expBarHandler.SetValue(playerStatHandler.PlayerData.CurrentExperience, playerStatHandler.PlayerData.NextLevelUpExperience);

        playerResourceHandler.OnHeal.AddListener(ShowHealth);
        playerResourceHandler.OnManaChanged.AddListener(ShowMana);
        playerStatHandler.PlayerData.OnExpChanged.AddListener(ShowExp);

        playerInteractionHandler.OnCanBeInteracted.AddListener(() => crossAirUI.ChangeToInteractionSprite());
        playerInteractionHandler.OnCanNotBeInteractedAnymore.AddListener(() => crossAirUI.ReturnToPreviouseSprite());

        playerPickUpHandler.OnCanBePickedUp.AddListener(() => crossAirUI.ChangeToPickUpSprite());
        playerPickUpHandler.OnCanNotBePickedUpAnymore.AddListener(() => crossAirUI.ReturnToPreviouseSprite());
        playerPickUpHandler.OnDrop.AddListener(() => crossAirUI.ReturnToPreviouseSprite());

        SInputManager.Instance.inputActions.Keyboard.Pause.performed += ChangeToPause;
        SInputManager.Instance.inputActions.Keyboard.Inventory.performed += ChangeToInventory;
        SInputManager.Instance.inputActions.Keyboard.Stats.performed += ChangeToStats;

        playerStatHandler.PlayerData.OnItemAdded.AddListener(OnItemAccuired);
        playerStatHandler.PlayerData.OnLevelUp.AddListener(ShowLevelUp);

        itemAccuiredText.gameObject.SetActive(false);
        HideHeal();
        HideDamage();
        HideLevelUp();
    }


    private void OnItemAccuired(ItemData item)
    {
        CancelInvoke(nameof(RemoveText));
        itemAccuiredText.gameObject.SetActive(true);

        if (item is ConsumbaleData consumbale) {

            itemAccuiredText.text = "x" + consumbale.Amount + " " + consumbale.ItemName + " Accuired";
        }
        else if(item is GoldItem gold)
        {
            itemAccuiredText.text = "x" + gold.GoldAmount + " " + gold.ItemName + " Accuired";
        }
        else
        {
            itemAccuiredText.text = item.ItemName + " Accuired";
        }
        Invoke(nameof(RemoveText), accuiredTextShowTime);
    }

    private void RemoveText()
    {
        itemAccuiredText.gameObject.SetActive(false);
    }


    private void ChangeToStats(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.STATS_UI_STATENAME);
    }

    private void ChangeToPause(InputAction.CallbackContext context)
    {

        if (IsUIStateActive)
        {
            SUIManager.Instance.ChangeToUIState(SUIManager.MENU_UI_STATENAME);
        }
    }

    private void ChangeToInventory(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 1.0f;
        SGameManager.Instance.SetCursorVisibility(false, CursorLockMode.Locked);


        crossAirUI.SetValue(0);
        playerAnimationController.TriggerEquip();
        SGameManager.IsPaused = false;
        crossAirUI.ChangeSprite(PlayerData.CurrentEquipedWeapon);
        ShowHealth();

        itemUIHandler_1.Init(PlayerData.Consumable_1);
        itemUIHandler_2.Init(PlayerData.Consumable_2);
        itemUIHandler_3.Init(PlayerData.Consumable_3);

        spellHandler_1.Init(PlayerData.CurrentMagicSpell_1);
        spellHandler_2.Init(PlayerData.CurrentMagicSpell_2);
    }

    public override void OnExitUIState()
    {
        base.OnExitUIState();
        SGameManager.IsPaused = true;
    }


    private void ShowHeal()
    {
        healBorder.gameObject.SetActive(true);
        LeanTween.value(healBorder.gameObject, 0, 1, healBorderTime).setOnUpdate((float val) =>
        {
            Color c = healBorder.color;
            c.a = val;
            healBorder.color = c;
        }).setOnComplete(RevertHeal).setEase(borderType);
    }

    private void RevertHeal()
    {
        LeanTween.value(healBorder.gameObject, 1, 0, healBorderTime).setOnUpdate((float val) =>
        {
            Color c = healBorder.color;
            c.a = val;
            healBorder.color = c;
        }).setOnComplete(HideHeal).setEase(borderType);
    }

    private void HideHeal()
    {
        healBorder.gameObject.SetActive(false);
    }


    private void ShowDamage()
    {
        damageBorder.gameObject.SetActive(true);
        LeanTween.value(damageBorder.gameObject, 0, 1, damageBorderTime).setOnUpdate((float val) =>
        {
            Color c = damageBorder.color;
            c.a = val;
            damageBorder.color = c;
        }).setOnComplete(RevertDamage).setEase(borderType);
    }

    private void RevertDamage()
    {
        LeanTween.value(damageBorder.gameObject, 1, 0, damageBorderTime).setOnUpdate((float val) =>
        {
            Color c = damageBorder.color;
            c.a = val;
            damageBorder.color = c;
        }).setOnComplete(HideDamage).setEase(borderType);
    }

    private void HideDamage()
    {
        damageBorder.gameObject.SetActive(false);
    }


    private void ShowLevelUp(int level)
    {
        levelUpBorder.gameObject.SetActive(true);
        LeanTween.value(levelUpBorder.gameObject, 0, 1, levelUpBorderTime).setOnUpdate((float val) =>
        {
            Color c = levelUpBorder.color;
            c.a = val;
            levelUpBorder.color = c;
        }).setOnComplete(RevertLevelUp).setEase(borderType);
    }

    private void RevertLevelUp()
    {
        LeanTween.value(levelUpBorder.gameObject, 1, 0, levelUpBorderTime).setOnUpdate((float val) =>
        {
            Color c = levelUpBorder.color;
            c.a = val;
            levelUpBorder.color = c;
        }).setOnComplete(HideLevelUp).setEase(borderType);
    }

    private void HideLevelUp()
    {
        levelUpBorder.gameObject.SetActive(false);
    }

    private void SetChargeValue(float value)
    {
        crossAirUI.SetValue(value);
    }

    private void EndChargeValue(float value)
    {
        crossAirUI.ResetValue();
    }

    private void GetDamage(bool arg0, int arg1, float knockBack, Transform arg2)
    {
        UpdateHealth();
        ShowDamage();
    }
    private void ShowHealth()
    {
        healthBarHandler.SetValue(playerResourceHandler.currentHealth , playerResourceHandler.healthData.Health);
    }

    private void UpdateHealth()
    {
        healthBarHandler.SetValue(playerResourceHandler.currentHealth, playerResourceHandler.healthData.Health);
    }

    private void ShowExp(float arg0)
    {
      //  expBarHandler.SetValue(playerStatHandler.PlayerData.CurrentExperience, playerStatHandler.PlayerData.NextLevelUpExperience);
    }


    private void ShowMana(float arg0)
    {
        manaBarHandler.SetValue(playerResourceHandler.CurrentMana, playerStatHandler.PlayerData.Mana);
    }
}
