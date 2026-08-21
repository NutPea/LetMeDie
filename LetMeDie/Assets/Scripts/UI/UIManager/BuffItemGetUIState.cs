using Essentials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffItemGetUIState : UIStateComponent
{

   // [SerializeField] private TextMeshPro rarity;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image image;
    [SerializeField] private BattleLootTable buffItemTable;

    private BuffBattleLoot choosenBuffBattleLoot;
    [SerializeField] private Button accept;
    [SerializeField] private Button ban;

    PlayerData playerData;

    public override void OnInitUIState()
    {
        base.OnInitUIState();
        buffItemTable.Init();
        accept.onClick.AddListener(Accept);
        ban.onClick.AddListener(Ban);

        playerData = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData;
    }

    private void Ban()
    {
        buffItemTable.AvailableLoots.Remove(choosenBuffBattleLoot);
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    private void Accept()
    {
        playerData.AddBuffBattleLoot(choosenBuffBattleLoot);
        SUIManager.Instance.ChangeToUIState(SUIManager.GAME_UI_STATENAME);
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        choosenBuffBattleLoot = GetBuffBattleLoot();
        name.text = choosenBuffBattleLoot.Name;
        description.text = choosenBuffBattleLoot.Description;
        image.sprite = choosenBuffBattleLoot.Icon;

        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        Time.timeScale = 0.0f;

    }

    public override void OnExitUIState()
    {
        base.OnExitUIState();
        Time.timeScale = 1.0f;
    }
    public BuffBattleLoot GetBuffBattleLoot()
    {
        return buffItemTable.AvailableLoots[Random.Range(0,buffItemTable.AvailableLoots.Count -1)] as BuffBattleLoot;
    }

}
