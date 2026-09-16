using Essentials;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SGameProgressionManager : MonoBehaviour
{
    public static SGameProgressionManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Progression = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int Progression = 0;
    public enum RewardTyp
    {
        None,
        Spell,
        LevelUp,
        Item
    }
    private RewardTyp currentRewardTyp = RewardTyp.Spell;
    public RewardTyp CurrentRewardTyp { get { return currentRewardTyp; } set { currentRewardTyp = value; } }

    [Header("BoonPrefabs")]
    [SerializeField] private GameObject spellBoonPrefab;
    [SerializeField] private GameObject levelBoonPrefab;
    [SerializeField] private GameObject itemBoonPrefab;

    [Header("RewardItems")]
    [SerializeField] private Sprite spellBoonIcon;
    [SerializeField] private Sprite levelBoonIcon;
    [SerializeField] private Sprite itemBoonIcon;

    [Header("LevelName")]
    [SerializeField] private List<string> easyLevelNames = new();
    [SerializeField] private List<string> middleLevelNames = new();
    [SerializeField] private List<string> hardLevelNames = new();
    [SerializeField] private string bossLevelName = "";


    private int killedEnemies = 0;
    public int KilledEnemies => killedEnemies;
    public UnityEvent<int> OnEnemyKilled = new();


    public GameObject GetBoonPrefab()
    {
        switch (currentRewardTyp) {
            case RewardTyp.LevelUp: return levelBoonPrefab;
            case RewardTyp.Spell: return spellBoonPrefab;
            case RewardTyp.Item: return itemBoonPrefab;
        }

        return null;
    }

    public Sprite GetBoonIcon(RewardTyp typ)
    {
        switch (typ)
        {
            case RewardTyp.LevelUp: return levelBoonIcon;
            case RewardTyp.Spell: return spellBoonIcon;
            case RewardTyp.Item: return itemBoonIcon;
        }
        return null;
    }

    public void LoadNextRoom(RewardTyp typ)
    {
        currentRewardTyp = typ;
        Progression++;
        SLoadManager.Instance.LoadScene(GetNextLevel());
    }


    public void EnemyDied()
    {
        killedEnemies++;
        OnEnemyKilled.Invoke(killedEnemies);
    }
    private string GetNextLevel()
    {
        if (Progression <= 2)
        {
            return GetEasyLevelName();
        }
        else if (Progression > 2 && Progression <= 5) {
            return GetMiddleLevelName();
        }else if(Progression > 5 && Progression < 8)
        {
            return GetHardLevelName();
        }
        else
        {
            return bossLevelName;
        }
    }

    private string GetHardLevelName()
    {
        return easyLevelNames[UnityEngine.Random.Range(0, easyLevelNames.Count)];
    }

    private string GetMiddleLevelName()
    {
        return middleLevelNames[UnityEngine.Random.Range(0, middleLevelNames.Count)];
    }

    private string GetEasyLevelName()
    {
        return hardLevelNames[UnityEngine.Random.Range(0, hardLevelNames.Count)];
    }
}
