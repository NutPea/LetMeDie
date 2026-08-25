using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class GiveExpOnDeathHealthManagerExtension : MonoBehaviour
{
    [SerializeField] private int commonAmount = 10;
    [SerializeField] private int uncommoonAmount = 30;
    [SerializeField] private int rareAmount = 75;
    [SerializeField] private int epicAmount = 200;
    [SerializeField] private int legendaryAmount = 500;

    private HealthManager healthManager;
    [SerializeField] private float dropPercentage = 0.5f;

    private int experienceAmount;




    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(SpawnDrops);

        BattleLoot.LootRarity rarity = SpellLevelUpUIState.GetRarity(0.65f, 0.25f, 0.08f, 0.18f, 0.02f);
        switch (rarity)
        {
            case BattleLoot.LootRarity.Common: experienceAmount = commonAmount; break;
            case BattleLoot.LootRarity.Uncommen:  experienceAmount = uncommoonAmount; break;
            case BattleLoot.LootRarity.Rare: experienceAmount = rareAmount; break;
            case BattleLoot.LootRarity.Epic: experienceAmount = epicAmount; break;
            case BattleLoot.LootRarity.Legendary:  experienceAmount = legendaryAmount; break;
        }
    }

    private void SpawnDrops(GameObject arg0)
    {
        SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData.AddExperience(experienceAmount);
    }


}
