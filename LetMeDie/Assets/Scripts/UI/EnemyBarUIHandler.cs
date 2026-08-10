using System;
using TMPro;
using UnityEngine;

public class EnemyBarUIHandler : MonoBehaviour
{

    [SerializeField] private BarUIHandler barUIHandler;
    [SerializeField] private GameObject enemyHealthParent;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timeUntilRemove = 4f;
    void Start()
    {
        SGameManager.Instance.OnEnemyDamage.AddListener(ShowDamage);
        HideEverything();
    }

    private void ShowDamage(HealthManager enemyHealth,bool isDead)
    {
        if (isDead)
        {
            HideEverything();
            return;
        }

        CancelInvoke(nameof(HideEverything));
        enemyHealthParent.SetActive(true);
        text.text = enemyHealth.healthData.Name;
        barUIHandler.SetValue(enemyHealth.currentHealth,enemyHealth.healthData.Health);
        Invoke(nameof(HideEverything),timeUntilRemove);
    }

    private void HideEverything()
    {
        enemyHealthParent.SetActive(false);
    }


}
