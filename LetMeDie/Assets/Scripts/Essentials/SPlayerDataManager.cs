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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
