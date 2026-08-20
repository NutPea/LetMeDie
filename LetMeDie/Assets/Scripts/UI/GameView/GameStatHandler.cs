using System;
using TMPro;
using UnityEngine;

public class GameStatHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI killedEnemysText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Transform expPivotText;

    private PlayerStatHandler playerStatHandler;
    private void Start()
    {
        SGameManager.Instance.OnEnemyKilled.AddListener(KilledEnemiesUpdate);
        playerStatHandler = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>();
        playerStatHandler.PlayerData.OnLevelUp.AddListener(ShowLevelUp);
        playerStatHandler.PlayerData.OnExpChanged.AddListener(ExpChange);
        playerStatHandler.PlayerData.OnGoldChange.AddListener(GoldChange);

        playerLevelText.text = 0.ToString();
        goldText.text = 0.ToString();
        killedEnemysText.text = 0.ToString();
        playerLevelText.text = "Lvl :" + 1.ToString();
        ExpChange(0.0f);

    }

    private void GoldChange(int goldAmount)
    {
        goldText.text = goldAmount.ToString();
    }

    private void Update()
    {
        WriteTimer(SGameManager.Instance.RemainingGameTime);
    }

    private void WriteTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private void ExpChange(float percent)
    {
        expPivotText.transform.localScale = new Vector3(percent, 1, 1);
    }

    private void ShowLevelUp(int levelUp)
    {
        playerLevelText.text = "Lvl :" + levelUp.ToString();
    }

    private void KilledEnemiesUpdate(int killedEnemys)
    {
        killedEnemysText.text = killedEnemys.ToString();
    }
}
