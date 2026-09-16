using Essentials;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUIState : UIStateComponent
{

    [SerializeField] private TextMeshProUGUI kills;
    [SerializeField] private Button backToMainMenu;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        backToMainMenu.onClick.AddListener(ChangeToMainMenu);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        kills.text = "Kills " + SGameProgressionManager.Instance.KilledEnemies;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        Time.timeScale = 0f;
    }

    private void ChangeToMainMenu()
    {
        Time.timeScale = 1f;
        SLoadManager.Instance.LoadScene(SLoadManager.LevelName.MainMenu);
    }
}
