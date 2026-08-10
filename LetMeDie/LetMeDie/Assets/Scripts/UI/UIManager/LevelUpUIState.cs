using Essentials;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PixelCrushers.AnimatorSaver;

public class LevelUpUIState : UIStateComponent
{

    private PlayerStatHandler statHandler;
    private PlayerMovementController playerMovementController;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI skillPointsAmountText;
    [SerializeField] private TextMeshProUGUI description;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI meeleDamageText;
    [SerializeField] private TextMeshProUGUI jumpHeightText;
    [SerializeField] private TextMeshProUGUI movementSpeedText;

    [Header("Selection")]

    [SerializeField] private LevelUpSelection strengthSelection;
    [SerializeField] private LevelUpSelection dexteritySelection;
    [SerializeField] private LevelUpSelection intelligenceSelection;
    [SerializeField] private LevelUpSelection resilienceSelection;
    [SerializeField] private LevelUpSelection luckSelection;
    [SerializeField] private LevelUpSelection speedSelection;


    [SerializeField] private bool hasEnough;

    [SerializeField] private Button apply;
    [SerializeField] private Button back;

    private PlayerData beforePlayerData;
    private PlayerData currentPlayerData;

    private int availableSkillPoints;
    private int currentSkillPoints;

    private Color increaseColor;

    [SerializeField] private bool changeToGameAfterApply;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        statHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();
        playerMovementController = statHandler.GetComponent<PlayerMovementController>();
        increaseColor = SGameManager.Instance.IncreaseColor;

        strengthSelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);
        dexteritySelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);
        intelligenceSelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);
        resilienceSelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);
        luckSelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);
        speedSelection.GetComponent<LevelUpDescriptionSelection>().OnDescriptionChange.AddListener(ChangeDescription);

        strengthSelection.Init(HasEnoughSkillPoints);
        dexteritySelection.Init(HasEnoughSkillPoints);
        intelligenceSelection.Init(HasEnoughSkillPoints);
        resilienceSelection.Init(HasEnoughSkillPoints);
        luckSelection.Init(HasEnoughSkillPoints);
        speedSelection.Init(HasEnoughSkillPoints);


        back.onClick.AddListener(Back);
        apply.onClick.AddListener(Apply);

        strengthSelection.OnValueIncrease.AddListener(ChangeStrength);
        strengthSelection.OnValueDecrease.AddListener(ChangeStrength);
        strengthSelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        strengthSelection.OnValueDecrease.AddListener(IncreaseSkillPoint);

        dexteritySelection.OnValueIncrease.AddListener(ChangeDexterity);
        dexteritySelection.OnValueDecrease.AddListener(ChangeDexterity);
        dexteritySelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        dexteritySelection.OnValueDecrease.AddListener(IncreaseSkillPoint);


        intelligenceSelection.OnValueIncrease.AddListener(ChangeIntelligence);
        intelligenceSelection.OnValueDecrease.AddListener(ChangeIntelligence);
        intelligenceSelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        intelligenceSelection.OnValueDecrease.AddListener(IncreaseSkillPoint);


        resilienceSelection.OnValueIncrease.AddListener(ChangeResilience);
        resilienceSelection.OnValueDecrease.AddListener(ChangeResilience);
        resilienceSelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        resilienceSelection.OnValueDecrease.AddListener(IncreaseSkillPoint);


        luckSelection.OnValueIncrease.AddListener(ChangeLuck);
        luckSelection.OnValueDecrease.AddListener(ChangeLuck);
        luckSelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        luckSelection.OnValueDecrease.AddListener(IncreaseSkillPoint);

        speedSelection.OnValueIncrease.AddListener(ChangeSpeed);
        speedSelection.OnValueDecrease.AddListener(ChangeSpeed);
        speedSelection.OnValueIncrease.AddListener(ReduceSkillPoint);
        speedSelection.OnValueDecrease.AddListener(IncreaseSkillPoint);

        ChangeDescription(strengthSelection.GetComponent<LevelUpDescriptionSelection>().Description);

    }

    public void ChangeDescription(string descriptionText)
    {
        description.text = descriptionText;
    }

    private void ChangeStrength(int value)
    {
        currentPlayerData.Strength = value;
        UpdateStatText();
    }


    private void ChangeDexterity(int value)
    {
        currentPlayerData.Dexterity = value;
        UpdateStatText();
    }
    private void ChangeIntelligence(int value)
    {
        currentPlayerData.Intelligence = value;
        UpdateStatText();
    }

    private void ChangeResilience(int value)
    {
        currentPlayerData.Resilience = value;
        UpdateStatText();
    }

    private void ChangeLuck(int value)
    {
        currentPlayerData.Luck = value;
        UpdateStatText();
    }

    private void ChangeSpeed(int value)
    {
        currentPlayerData.Speed = value;
        UpdateStatText();
    }



    private void IncreaseSkillPoint(int value)
    {
        currentSkillPoints++;
        if (currentSkillPoints > availableSkillPoints)
        {
            currentSkillPoints = availableSkillPoints;
        }
        skillPointsAmountText.text = currentSkillPoints.ToString();
    }

    private void ReduceSkillPoint(int value)
    {
        currentSkillPoints--;
        if(currentSkillPoints < 0)
        {
            currentSkillPoints = 0;
        }
        skillPointsAmountText.text = currentSkillPoints.ToString();
    }

    private void Apply()
    {
        if(currentSkillPoints > 0)
        {
            return;
        }
        currentPlayerData.LevelUp();
        if (changeToGameAfterApply)
        {
            SUIManager.Instance.ChangeToUIState(SUIManager.CHECKPOINT_UI_STATENAME);
        }
        else
        {
            Time.timeScale = 1;
            SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
        }
    }

    private void Back()
    {
        currentPlayerData.ReadPlayerDataStats(beforePlayerData);
        SUIManager.Instance.ChangeToUIState(SUIManager.CHECKPOINT_UI_STATENAME);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        strengthSelection.ChangeValuesOnNewUI(statHandler.PlayerData.Strength);
        dexteritySelection.ChangeValuesOnNewUI(statHandler.PlayerData.Dexterity);
        intelligenceSelection.ChangeValuesOnNewUI(statHandler.PlayerData.Intelligence);
        resilienceSelection.ChangeValuesOnNewUI(statHandler.PlayerData.Resilience);
        luckSelection.ChangeValuesOnNewUI(statHandler.PlayerData.Luck);
        speedSelection.ChangeValuesOnNewUI(statHandler.PlayerData.Speed);

        beforePlayerData = Instantiate(statHandler.PlayerData);
        currentPlayerData = statHandler.PlayerData;
        availableSkillPoints = statHandler.PlayerData.CurrentLevel - statHandler.PlayerData.CharacterLevel;
        currentSkillPoints = availableSkillPoints;

        levelText.text = "Level " + statHandler.PlayerData.CurrentLevel;
        skillPointsAmountText.text = "Skillpoints: " + availableSkillPoints.ToString();
        UpdateStatText();
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        Time.timeScale = 0;
    }



    public bool HasEnoughSkillPoints()
    {
        return currentSkillPoints > 0;
    }

    public void UpdateStatText()
    {
        float currentMovementSpeed = PlayerData.CalculateMovementSpeed(playerMovementController.baseMovementSpeed, currentPlayerData.Speed);
        float beforeMovementSpeed = PlayerData.CalculateMovementSpeed(playerMovementController.baseMovementSpeed, beforePlayerData.Speed);

        float currentJumpHeight = PlayerData.CalculateMovementSpeed(playerMovementController.baseJumpForce, currentPlayerData.Dexterity);
        float beforeJumpHeight = PlayerData.CalculateMovementSpeed(playerMovementController.baseJumpForce, beforePlayerData.Dexterity);

        float currentMeleeDamage = PlayerData.CalculateMelleDamage(currentPlayerData.Strength);
        float beforeMeleeDamage = PlayerData.CalculateMelleDamage(beforePlayerData.Strength);

        healthText.color = currentPlayerData.Health > beforePlayerData.Health ? increaseColor : Color.white;
        manaText.color = currentPlayerData.Mana > beforePlayerData.Mana ? increaseColor : Color.white;
        movementSpeedText.color = currentMovementSpeed > beforeMovementSpeed ? increaseColor : Color.white;
        jumpHeightText.color = currentJumpHeight > beforeJumpHeight ? increaseColor : Color.white;
        meeleDamageText.color = currentMeleeDamage > beforeMeleeDamage ? increaseColor : Color.white;

        healthText.text = "Health " + currentPlayerData.Health.ToString();
        manaText.text ="Mana "+ currentPlayerData.Mana.ToString();
        movementSpeedText.text = "Movement Speed " +PlayerData.CalculateMovementSpeed(playerMovementController.baseMovementSpeed,currentPlayerData.Speed).ToString("F1");
        jumpHeightText.text = "Jump Height " + PlayerData.CalculateMovementSpeed(playerMovementController.baseJumpForce, currentPlayerData.Dexterity).ToString("F1");
        meeleDamageText.text = "Base Melee Damage " +  PlayerData.CalculateMelleDamage(currentPlayerData.Strength).ToString("F1");
    }


}
