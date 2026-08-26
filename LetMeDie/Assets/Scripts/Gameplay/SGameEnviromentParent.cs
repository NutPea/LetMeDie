using UnityEngine;

public class SGameEnviromentParent : MonoBehaviour
{
    public static SGameEnviromentParent Instance;
    public Transform PlayerSpawnPosition;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("That should not be!");
        }
    }
}
