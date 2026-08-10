using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SGameManager : MonoBehaviour
{
    public bool PlayerIsDead;
    public static SGameManager Instance;
    public GameObject PlayerBody;
    public static bool IsPaused;
    public static bool IsInDialog;

    [HideInInspector] public UnityEvent<HealthManager,bool> OnEnemyDamage = new();

    public Color IncreaseColor;

    [Header("Debug")]
    [SerializeField] private GameObject sphere;
    public GameObject Sphere => sphere;


    private void Awake()
    {
        if (Instance == null) {
        
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.75f);
        SetCursorVisibility(false, CursorLockMode.Locked);
    }

    public void SetCursorVisibility(bool visible , CursorLockMode cursorLockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = cursorLockMode;
    }



}
