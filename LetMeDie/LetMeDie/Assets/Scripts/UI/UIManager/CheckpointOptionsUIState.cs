using Essentials;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointOptionsUIState : UIStateComponent
{
    private HealthManager healthManager;

    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button backButton;

    

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        levelUpButton.onClick.AddListener(ChangeToLevelUp);
        backButton.onClick.AddListener(Back);

        healthManager = SGameManager.Instance.PlayerBody.GetComponent<HealthManager>();
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        healthManager.Recover();
    }
    private void Back()
    {
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    private void ChangeToLevelUp()
    {
        SUIManager.Instance.ChangeToUIState(SUIManager.LEVEL_UP_UI_STATENAME);
    }
}
