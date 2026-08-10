using System;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class GiveExpOnDeathHealthManagerExternsion : MonoBehaviour
{

    [SerializeField] private int amountOfExp;
    private HealthManager healthManager;
    void Start()
    {
        healthManager = GetComponent<HealthManager> ();
        healthManager.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(GameObject diedObject)
    {
        GameObject player = SGameManager.Instance.PlayerBody;
        player.GetComponent<PlayerStatHandler>().PlayerData.AddExperience(amountOfExp);
    }

}
