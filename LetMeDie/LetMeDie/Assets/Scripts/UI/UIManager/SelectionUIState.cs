using Essentials;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUIState : UIStateComponent
{
    private ItemData currentSelectedItem;

    [SerializeField] private DescriptionUI descriptionUI;
    [SerializeField] private ItemSelectionButton nothingButtons;
    [SerializeField] private List<ItemSelectionButton> itemButtons;


    [SerializeField] private Button equip;
    [SerializeField] private Button back;

    [SerializeField] private EquipmentUIStateComponent equipmentUIState;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        foreach (ItemSelectionButton itemButton in itemButtons) {
            itemButton.Init();
            itemButton.OnSetItem.AddListener(SetDecription);
        }
        nothingButtons.Init();
        nothingButtons.OnSetItem.AddListener(SetDecription);
        equip.onClick.AddListener(EquipSelectedItem);
        back.onClick.AddListener(Back);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        currentSelectedItem = null;

    }

    private void EquipSelectedItem()
    {
        equipmentUIState.Equip(currentSelectedItem); 
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }

    private void Back()
    {
        currentSelectedItem = null;
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }


    private void SetDecription(ItemData itemData)
    {
        descriptionUI.SetItem(itemData);
        currentSelectedItem = itemData;
    }

    public void SetItems(List<ItemData> itemDatas,ItemData toSelectItem)
    {
        itemButtons.ForEach((n) => n.gameObject.SetActive(false));
        for(int i = 0; i < itemDatas.Count; i++)
        {
            itemButtons[i].SetItem(itemDatas[i]);
            itemButtons[i].gameObject.SetActive(true);
        }
        SetDecription(toSelectItem);
        SUIManager.Instance.ChangeToUIState(SUIManager.SELECTION_UI_STATENAME);
    }


}
