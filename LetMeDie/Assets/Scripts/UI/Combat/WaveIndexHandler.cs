using UnityEngine;
using UnityEngine.UI;

public class WaveIndexHandler : MonoBehaviour
{
    [SerializeField] private Image waveIndexImage;
    [SerializeField] private Sprite combatRound;
    [SerializeField] private Sprite lootRound;
    [SerializeField] private Sprite bossRound;

    public void SetNothing()
    {
        gameObject.SetActive(false);
    }

    public void SetWave(Wave wave)
    {
        gameObject.SetActive(true);
        switch (wave.waveType) {
            case Wave.WaveType.Combat: waveIndexImage.sprite = combatRound; break;
            case Wave.WaveType.Loot: waveIndexImage.sprite = lootRound; break;
            case Wave.WaveType.Boss: waveIndexImage.sprite = bossRound; break;
        }
    }
}
