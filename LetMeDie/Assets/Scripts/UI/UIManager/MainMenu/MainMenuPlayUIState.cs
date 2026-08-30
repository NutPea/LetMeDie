using Essentials;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPlayUIState : UIStateComponent
{
    [SerializeField] private List<PlayerData> playerDatas = new();
    [SerializeField] private List<CharacterSelectionButton> characterSelectionButton = new();

    [SerializeField] private CharacterDescriptionView characterDescriptionView;
    [SerializeField] private Button startLevel;
    [SerializeField] private Button back;

    private PlayerData currentPlayerData;
    public override void OnInitUIState()
    {
        base.OnInitUIState();
        characterSelectionButton.ForEach((b) => b.gameObject.SetActive(false));
        for(int i = 0; i< playerDatas.Count; i++)
        {
            characterSelectionButton[i].Setup(playerDatas[i]);
            characterSelectionButton[i].OnCharacterSelect.AddListener(SetPlayer);
            characterSelectionButton[i].gameObject.SetActive(true);

        }
        startLevel.onClick.AddListener(StartLevel);
        back.onClick.AddListener(Back);
        characterDescriptionView.ShowCharacter(playerDatas[0]);
    }

    private void StartLevel()
    {
        SPlayerDataManager.Instance.CurrentPlayerData = currentPlayerData;
        ChooseLevel();
        Debug.Log("StartLevel");
    }

    private void ChooseLevel()
    {
        SLoadManager.Instance.LoadScene(SLoadManager.LevelName.DungeonScene_1);
    }

    private void Back()
    {
        SUIManager.Instance.ChangeToUIState("Main");
    }

    public void SetPlayer(PlayerData data)
    {
        characterDescriptionView.ShowCharacter(data);
        currentPlayerData = data;
    }
}
