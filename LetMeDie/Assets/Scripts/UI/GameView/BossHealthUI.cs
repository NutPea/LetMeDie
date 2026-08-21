using System;
using TMPro;
using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBackground;
    [SerializeField] private BarUIHandler healthBar;
    private HealthManager bossHealth;
    [SerializeField] private TextMeshProUGUI bossName; 

    private void Start()
    {
        SGameManager.Instance.OnBossRegistered.AddListener(ShowBoss);
        healthBackground.SetActive(false);
    }

    private void ShowBoss(HealthManager bossHealth)
    {
        this.bossHealth = bossHealth;
        bossHealth.OnDamaged.AddListener(UpdateHealth);
        bossHealth.OnDeath.AddListener(Death);
        bossName.text = bossHealth.healthData.Name;
        healthBackground.SetActive(true);
        healthBar.SetValue(bossHealth.currentHealth, bossHealth.healthData.Health);
    }

    private void Death(GameObject arg0)
    {
        healthBackground.SetActive(false);
    }

    private void UpdateHealth(bool arg0, int arg1, Transform arg2)
    {
        healthBar.SetValue(bossHealth.currentHealth, bossHealth.healthData.Health);
    }
}
