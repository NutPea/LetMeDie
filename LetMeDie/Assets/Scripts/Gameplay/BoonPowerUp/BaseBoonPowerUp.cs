using UnityEngine;
using UnityEngine.Events;

public class BaseBoonPowerUp : MonoBehaviour, IInteractable
{

    [HideInInspector] public UnityEvent OnPickUp = new UnityEvent();
    public void OnInteract(Transform player)
    {
        OnBoonPickUp();
        OnPickUp.Invoke();
    }

    public virtual void OnBoonPickUp()
    {
        Destroy(gameObject);
    }
}
