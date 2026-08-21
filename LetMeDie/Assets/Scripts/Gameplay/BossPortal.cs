using UnityEngine;

public class BossPortal : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject bossPrefab;

    private bool hasBeenSpawned;

    public void OnInteract(Transform player)
    {
        if (hasBeenSpawned)
        {
            return;
        }
        GameObject boss = Instantiate(bossPrefab);
        boss.transform.position = spawnPosition.position;
        boss.transform.forward = spawnPosition.forward;

        if(boss.TryGetComponent(out BaseEnemyController baseEnemyController))
        {
            baseEnemyController.SetAggro();
        }


        if (boss.TryGetComponent(out BaseEnemyMovement baseEnemyMovement))
        {
            baseEnemyMovement.OnAggro();
        }
        hasBeenSpawned  = true;

    }
}
