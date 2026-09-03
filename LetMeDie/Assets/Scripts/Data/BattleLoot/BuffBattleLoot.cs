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

    public void StartTempBuff()
    {
        CurrentTemporaryBuffTime = temporaryBuffTime;
    }

    public virtual void BuffBattleLootAdded(GameObject player , PlayerData data)
    {
        this.playerData = data;
    }

    public virtual void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {

    }

}
