using UnityEngine;

public class HealthPowerUp : PowerUp
{
    [SerializeField] private float healPercentageAmount = 0.2f;


    protected override void PickUp(GameObject player)
    {
        PlayerResourceHandler resourceHandler = player.GetComponent<PlayerResourceHandler>();
        float healAmount = resourceHandler.healthData.Health * healPercentageAmount;
        resourceHandler.Heal(Mathf.CeilToInt(healAmount));
    }
}
