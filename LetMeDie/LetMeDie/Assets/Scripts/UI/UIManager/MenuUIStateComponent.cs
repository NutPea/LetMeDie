using Essentials;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuUIStateComponent : UIStateComponent
{
    [SerializeField] private Button statButton;
    [SerializeField] private Button equipmentButton;


    public override void OnInitUIState()
    {
        base.OnInitUIState();
        statButton.onClick.AddListener(ChangeToStats);
        equipmentButton.onClick.AddListener(ChangeToEquipment);

        SInputManager.Instance.inputActions.Keyboard.Pause.performed += ChangeToPauseOrGame;
        SInputManager.Instance.inputActions.Keyboard.Stats.performed += ctx => ChangeToStats();
        SInputManager.Instance.inputActions.Keyboard.Inventory.performed += ctx => ChangeToEquipment();
        IsUIStateActive = false;
    }



    private void ChangeToPauseOrGame(InputAction.CallbackContext context)
    {
        if (!IsUIStateActive)
        {
            return;
        }

        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        IsUIStateActive = false;
        StartCoroutine(ResetUIStateActive());
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_OpenMenu);
    }

    IEnumerator ResetUIStateActive()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        IsUIStateActive = true;
    }


    private void ChangeToStats()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.STATS_UI_STATENAME);
    }

    private void ChangeToEquipment()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }
}
