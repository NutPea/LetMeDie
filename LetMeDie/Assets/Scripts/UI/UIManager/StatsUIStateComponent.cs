using Essentials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIStateComponent : UIStateComponent
{
    private PlayerStatHandler playerStatHandler;
    private PlayerData PlayerData => playerStatHandler.PlayerData;

    [SerializeField] private Button equipmentButton;
    [SerializeField] private Button menuButton;


    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI inteligenceText;
    [SerializeField] private TextMeshProUGUI resilienceText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI luckText;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;

    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI experience;
    [SerializeField] private TextMeshProUGUI gold;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        menuButton.onClick.AddListener(ChangeToMenu);
        equipmentButton.onClick.AddListener(ChangeToEquipment);
        playerStatHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();

        SInputManager.Instance.inputActions.Keyboard.Pause.performed += ctx => ChangeToGame();
        SInputManager.Instance.inputActions.Keyboard.Inventory.performed += ctx => ChangeToEquipment();
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);

        UpdateStats();
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_OpenMenu);
    }

    private void UpdateStats()
    {
        /*
        strengthText.text = PlayerData.Strength.ToString();
        dexterityText.text = PlayerData.Dexterity.ToString();
        inteligenceText.text = PlayerData.Intelligence.ToString();
        resilienceText.text = PlayerData.Resilience.ToString();
        speedText.text = PlayerData.Speed.ToString();
        luckText.text = PlayerData.Luck.ToString();

        healthText.text = PlayerData.Health.ToString();

        gold.text = PlayerData.GoldItem.GoldAmount.ToString();

        level.text = PlayerData.CurrentLevel.ToString();
        float ExperiencePercent = PlayerData.ExperiencePercent * 100;
        experience.text = ExperiencePercent.ToString("F0") + " %";

        */
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

    private void ChangeToEquipment()
    {
        if (!IsUIStateActive)
        {
            return;
        }
        SUIManager.Instance.ChangeToUIState(SUIManager.EQUIPMENT_UI_STATENAME);
    }

}
