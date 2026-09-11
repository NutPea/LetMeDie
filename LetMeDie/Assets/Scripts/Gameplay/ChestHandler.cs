using UnityEngine;

public class ChestHandler : MonoBehaviour , IInteractable
{
    [SerializeReference] private bool chestIsFree = false;
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
                if (chestIsFree) {
                    playerChestHandler.CanOpenFreeCest(this);
                }
                else
                {
                    playerChestHandler.CanOpenChest(this);
                }
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
