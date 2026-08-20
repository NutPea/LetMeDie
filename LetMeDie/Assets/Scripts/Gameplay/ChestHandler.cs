using UnityEngine;

public class ChestHandler : MonoBehaviour , IInteractable
{
    public void OnInteract(Transform player)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.TryGetComponent(out PlayerChestHandler playerChestHandler))
            {
                playerChestHandler.CanOpenChest(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerChestHandler playerChestHandler))
            {
                playerChestHandler.CanNotOpenChest(this);
            }
        }
    }
}
