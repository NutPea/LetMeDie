using UnityEngine;

public class SPlayerDataManager : MonoBehaviour
{
    public static SPlayerDataManager Instance;
    public PlayerData CurrentPlayerData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
