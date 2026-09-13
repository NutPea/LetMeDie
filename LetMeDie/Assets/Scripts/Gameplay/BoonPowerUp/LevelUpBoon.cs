using UnityEngine;

public class LevelUpBoon : BaseBoonPowerUp
{

    public override void OnBoonPickUp()
    {
        base.OnBoonPickUp();
        SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData.ForceLevelUp();
    }


}
