using UnityEngine;

public class BuffBattleLoot : BattleLoot
{
    protected PlayerData playerData;

    [SerializeField] private bool showsAsItem = true;
    public bool ShowsAsItem => showsAsItem;

    [Header("Temp")]
    [SerializeField] private float temporaryBuffTime = 0.0f;
    public float TemporaryBuffTime => temporaryBuffTime;    
    [HideInInspector] public float CurrentTemporaryBuffTime = 0.0f;
    public bool IsTempBuffActiv => CurrentTemporaryBuffTime > 0.0f;

    protected float CurrentRarityModifier = 1.0f;

    [Header("Modifiers")]
    [SerializeField] private float minCommonRarityModRange = 0.0f;
    [SerializeField] private float maxCommonRarityModRange = 0.1f;

    [SerializeField] private float minUncommonRarityModRange = 0.2f;
    [SerializeField] private float maxUncommonRarityModRange = 0.3f;

    [SerializeField] private float minRareRarityModRange = 0.3f;
    [SerializeField] private float maxRareRarityModRange = 0.5f;

    [SerializeField] private float minEpicRarityModRange = 0.5f;
    [SerializeField] private float maxEpicRarityModRange = 1.0f;

    [SerializeField] private float minLegendaryRarityModRange = 1.5f;
    [SerializeField] private float maxLegendaryRarityModRange = 2.0f;

    public void StartTempBuff()
    {
        CurrentTemporaryBuffTime = temporaryBuffTime;
    }

    public override void CalculateValues()
    {
        base.CalculateValues();
        CurrentRarityModifier = RarityModifier();
    }

    public virtual void BuffBattleLootAdded(GameObject player , PlayerData data)
    {
        this.playerData = data;
    }

    public virtual void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {

    }

    protected float RarityModifier()
    {
        switch (lootRarity)
        {
            case LootRarity.Common: return Random.Range(minCommonRarityModRange, maxCommonRarityModRange); 
            case LootRarity.Uncommen: return Random.Range(minUncommonRarityModRange, maxUncommonRarityModRange);
            case LootRarity.Rare: return Random.Range(minRareRarityModRange, maxRareRarityModRange);
            case LootRarity.Epic: return  Random.Range(minEpicRarityModRange, maxEpicRarityModRange);
            case LootRarity.Legendary: return Random.Range(minLegendaryRarityModRange, maxLegendaryRarityModRange);
        }
        return 0f;
    }

}
