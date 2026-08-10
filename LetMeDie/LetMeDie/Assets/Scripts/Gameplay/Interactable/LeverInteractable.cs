using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : MonoBehaviour, IInteractable
{

    public bool value;

    public UnityEvent OnLeverOn = new();
    public UnityEvent OnLeverOff = new();



    private void Start()
    {
        if (value)
        {
            OnLeverOn.Invoke();
        }
        else
        {
            OnLeverOff.Invoke();
        }
    }
    public void OnInteract(Transform player)
    {
        if (value) {
            OnLeverOff.Invoke();
            value = false;
        } else {
            OnLeverOn.Invoke();
            value = true;
        }

    }
}
