using UnityEngine;

public class BossSpawnInteractable : MonoBehaviour,IInteractable
{

    private bool hasInteractable = false;
    public void OnInteract(Transform player)
    {
        if (!hasInteractable)
        {
            SGameManager.Instance.EndGame();
            hasInteractable=true;
        }
    }

    
}
