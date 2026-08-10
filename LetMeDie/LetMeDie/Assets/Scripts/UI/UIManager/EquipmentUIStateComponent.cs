using Essentials;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIStateComponent : UIStateComponent
{
    [SerializeField] private Button statButton;
    [SerializeField] private Button menuButton;

    private PlayerData playerData;
    [Header("Weapon")]
    [SerializeField] private InventoryItemButton weaponButton;
    [SerializeField] private PlayerWeaponEquiper playerWeaponEquiper;

    [Header("Items")]
    [SerializeField] private InventoryItemButton consumableButton_1;
    [SerializeField] private InventoryItemButton consumableButton_2;
    [SerializeField] private InventoryItemButton consumableButton_3;

    [Header("MagicSpells")]
    [SerializeField] private InventoryItemButton magicSpell_1;
    [SerializeField] private InventoryItemButton magicSpell_2;

    public enum SelectedItemType {None,Consumable,Weapon,Magic}
    private SelectedItemType selectedItemType = SelectedItemType.None;
    private int currentChangedConsumableIndex = -1;
    private int currentChangedMagicIndex = -1;
    public SelectionUIState selectionUIState;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        statButton.onClick.AddListener(ChangeToStats);
        menuButton.onClick.AddListener(ChangeToMenu);

        SInputManager.Instance.inputActions.Keyboard.Pause.performed += ctx => ChangeToGame();
        SInputManager.Instance.inputActions.Keyboard.Stats.performed += ctx => ChangeToStats();

        consumableButton_1.button.onClick.AddListener(() => ChangeToItemSelection(0,playerData.Consumable_1));
        consumableButton_2.button.onClick.AddListener(() => ChangeToItemSelection(1, playerData.Consumable_2));
        consumableButton_3.button.onClick.AddListener(() => ChangeToItemSelection(2, playerData.Consumable_3));

        weaponButton.button.onClick.AddListener(() => ChangeToWeaponSelection(playerData.CurrentEquipedWeapon));

        magicSpell_1.button.onClick.AddListener(() => ChangeToMagicSelection(playerData.CurrentMagicSpell_1,0));
        magicSpell_2.button.onClick.AddListener(() => ChangeToMagicSelection(playerData.CurrentMagicSpell_2,1));
    }

    private void ChangeToMagicSelection(MagicSpell currentMagicSpell,int currentSelectedSpell)
    {
        currentChangedMagicIndex = currentSelectedSpell;
        selectionUIState.SetItems(playerData.MagicSpellInventory.Cast<ItemData>().ToList(), currentMagicSpell);
        selectedItemType = SelectedItemType.Magic;
    }

    private void ChangeToItemSelection(int index,ConsumbaleData consumbaleData)
    {
        currentChangedConsumableIndex = index;
        selectionUIState.SetItems(playerData.ConsumableInventory.Cast<ItemData>().ToList(), consumbaleData);
        selectedItemType = SelectedItemType.Consumable;
    }

    private void ChangeToWeaponSelection(WeaponData weaponData)
    {
        selectionUIState.SetItems(playerData.WeaponInventory.Cast<ItemData>().ToList(), weaponData);
        selectedItemType = SelectedItemType.Weapon;
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);

        if (playerData == null)
        {

            playerData = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData;
        }
        DrawButtons(playerData);
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_OpenMenu);
    }

    public void DrawButtons(PlayerData playerData)
    {
        weaponButton.SetItem(playerData.CurrentEquipedWeapon);

        consumableButton_1.SetItem(playerData.Consumable_1);
        consumableButton_2.SetItem(playerData.Consumable_2);
        consumableButton_3.SetItem(playerData.Consumable_3);

        magicSpell_1.SetItem(playerData.CurrentMagicSpell_1);
        magicSpell_2.SetItem(playerData.CurrentMagicSpell_2);
    }

    public void Equip(ItemData item)
    {
        switch (selectedItemType)
        {
            case SelectedItemType.Consumable: HandleConsumable(item); break;
            case SelectedItemType.Weapon: 
                playerData.CurrentEquipedWeapon = (WeaponData)item;
                playerWeaponEquiper.EquipWeapon(playerData.CurrentEquipedWeapon);
                break;
            case SelectedItemType.Magic:
                MagicSpell magicSpell = (MagicSpell)item;
                switch (currentChangedMagicIndex)
                {
                    case 0: playerWeaponEquiper.EquipSpell1(magicSpell); break;
                    case 1: playerWeaponEquiper.EquipSpell2(magicSpell); break;
                }
                break;
        }
      
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }

    private void HandleConsumable(ItemData item)
    {
        ConsumbaleData consumable = null;
        if (item is ConsumbaleData foundConsumable)
        {
            consumable = foundConsumable;
        }
        switch (currentChangedConsumableIndex)
        {
            case 0:
                playerData.Consumable_1 = consumable; break;
            case 1:
                playerData.Consumable_2 = consumable; break;
            case 2:
                playerData.Consumable_3 = consumable; break;
        }
    }

    private void ChangeToGame()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }
    private void ChangeToMenu()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.MENU_UI_STATENAME);
    }


    private void ChangeToStats()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.STATS_UI_STATENAME);
    }

}
