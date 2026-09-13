using Essentials;
using UnityEngine;

public class ItemBoon : BaseBoonPowerUp
{


    public override void OnBoonPickUp()
    {
        base.OnBoonPickUp();
        SUIManager.Instance.ChangeToUIState("GetItem");
    }
}
