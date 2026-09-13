using Essentials;
using UnityEngine;

public class SpellBoon : BaseBoonPowerUp 
{
    public override void OnBoonPickUp()
    {
        base.OnBoonPickUp();
        SUIManager.Instance.ChangeToUIState("SpellLevelUp");
    }
}
