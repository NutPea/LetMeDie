using Essentials;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

public class DialogUIState : UIStateComponent
{
    private PlayerAnimationController playerAnimationController;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        //GetComponentInChildren<StandardDialogueUI>().conversationUIElements.mainPanel.onClose.AddListener(CloseDialogUI);
       // playerAnimationController = SGameManager.Instance.PlayerBody.GetComponent<PlayerAnimationController>();
    }

    private void CloseDialogUI()
    {
        SUIManager.Instance.ChangeToUIState(SUIManager.Instance.PreviouseUIState.UIStateName);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();

        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);

        playerAnimationController.UnEquip();
        SGameManager.IsInDialog = true;
    }

    public override void OnExitUIState()
    {
        base.OnExitUIState();
        SGameManager.IsInDialog = false;
    }


}
