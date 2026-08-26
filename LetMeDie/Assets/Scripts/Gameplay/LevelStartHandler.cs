using System;
using Unity.AI.Navigation;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class LevelStartHandler : MonoBehaviour
{

    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private WaveSpawnManager waveSpawnManager;
    [SerializeField] private float radius = 100f;

    private GameObject playerBody;
    [SerializeField] private Transform playerSpawnPosition;

    [Header("Prefabs")]
    [SerializeField] private Transform potParentTransform;
    [SerializeField] private GameObject potPrefab;
    [SerializeField] private int amountOfPots = 100;

    [SerializeField] private Transform chestParentTransform;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private int amountOfChests = 30;

    [SerializeField] private Transform shrineParentTransform;
    [SerializeField] private GameObject shrinePrefab;
    [SerializeField] private int amountOfShrines = 20;

    bool isInPipeline;
    bool navMeshIsFinishedBaking;

    void Start()
    {
        playerBody = SGameManager.Instance.PlayerBody;
        StartPipeline();
    }

    private void StartPipeline()
    {
        PlacePrefabs();
       ChangeEnviroment();
        navMeshSurface.RemoveData();
        isInPipeline = true;
        navMeshIsFinishedBaking = false;
        navMeshSurface.BuildNavMesh();

    }

    private void NavMeshIsFinishedBaking()
    {
        playerBody.transform.position = playerSpawnPosition.transform.position;
        playerBody.transform.forward = playerSpawnPosition.transform.forward;
        waveSpawnManager.StartSpawning();
        Debug.Log("Remove Loading Screen");

    }

    private void ChangeEnviroment()
    {
        Transform enviroment = SGameEnviromentParent.Instance.transform;
        float xScaleValueRandomness = UnityEngine.Random.Range(0.0f,1.0f);
        float zScaleValueRandomness = UnityEngine.Random.Range(0.0f, 1.0f);
        int xScale = 1;
        int zScale = 1;
        if(xScaleValueRandomness > 0.5f)
        {
            xScale = -1;
        }

        if(zScaleValueRandomness > 0.5f)
        {
            zScale = -1;
        }

        enviroment.transform.localScale = new Vector3(xScale, 1, zScale);

    }

    private void Update()
    {
        if (isInPipeline)
        {
            if(navMeshSurface.navMeshData != null)
            {
                if (!navMeshIsFinishedBaking)
                {
                    NavMeshIsFinishedBaking();
                    navMeshIsFinishedBaking = true;
                }
            }
        }
    }

    private void PlacePrefabs()
    {
        potParentTransform.parent = SGameEnviromentParent.Instance.transform;
        chestParentTransform.parent = SGameEnviromentParent.Instance.transform;
        shrineParentTransform.parent = SGameEnviromentParent.Instance.transform;
        for (int i = 0; i < amountOfPots; i++)
        {
            GameObject pot = Instantiate(potPrefab,potParentTransform);
            pot.transform.position = FindRandomPosition();

        }

        for (int i = 0; i < amountOfChests; i++)
        {
            GameObject chest = Instantiate(chestPrefab,chestParentTransform);
            chest.transform.position = FindRandomPosition();
        }

        for (int i = 0; i < amountOfShrines; i++)
        {
            GameObject shrine = Instantiate(shrinePrefab,shrineParentTransform);
            shrine.transform.position = FindRandomPosition();
        }

    }

    private Vector3 FindRandomPosition()
    {
        NavMeshHit hit;
        int tryAmounts = 100;
        Vector3 finalPosition = Vector3.zero;
        for(int i = 0; i < tryAmounts; i++)
        {
            Vector3 randomDirection = transform.position + UnityEngine.Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                break;
            }
        }

        return finalPosition;
    }


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
