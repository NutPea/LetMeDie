using Essentials;
using UnityEngine;

public class CheckPointInteractable : MonoBehaviour, IInteractable
{
    public void OnInteract(Transform player)
    {
        SUIManager.Instance.ChangeToUIState(SUIManager.CHECKPOINT_UI_STATENAME);
    }
}
