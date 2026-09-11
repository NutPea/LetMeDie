using UnityEngine;

public class BaseBoonPowerUp : MonoBehaviour, IInteractable
{


    public void OnInteract(Transform player)
    {
        OnBoonPickUp();

    }

    public virtual void OnBoonPickUp()
    {

    }
}
