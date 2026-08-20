using UnityEngine;

public class ExpSphere : MonoBehaviour
{

    [SerializeField] private int commonAmount = 10;
    [SerializeField] private int uncommoonAmount = 30;
    [SerializeField] private int rareAmount = 75;
    [SerializeField] private int epicAmount = 200;
    [SerializeField] private int legendaryAmount = 500;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material commonMat;
    [SerializeField] private Material uncommonMat;
    [SerializeField] private Material rareMat;
    [SerializeField] private Material epicMat;
    [SerializeField] private Material legendaryMat;

    private int experienceAmount;

    private void Start()
    {
        BattleLoot.LootRarity rarity = SpellLevelUpUIState.GetRarity(0.65f, 0.25f, 0.08f, 0.18f, 0.02f);
        switch (rarity)
        {
            case BattleLoot.LootRarity.Common:meshRenderer.material = commonMat;  experienceAmount = commonAmount; break;
            case BattleLoot.LootRarity.Uncommen: meshRenderer.material = uncommonMat; experienceAmount = uncommoonAmount; break;
            case BattleLoot.LootRarity.Rare: meshRenderer.material = rareMat; experienceAmount = rareAmount; break;
            case BattleLoot.LootRarity.Epic: meshRenderer.material = epicMat; experienceAmount = epicAmount; break;
            case BattleLoot.LootRarity.Legendary: meshRenderer.material = legendaryMat; experienceAmount = legendaryAmount; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler statHandler = other.GetComponent<PlayerStatHandler>();
            if (statHandler != null) {
                statHandler.PlayerData.AddExperience(experienceAmount);
                Destroy(gameObject);
            }

        }
    }
}
