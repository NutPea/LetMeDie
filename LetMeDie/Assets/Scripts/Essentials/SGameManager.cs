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

    [SerializeField] public float GameDuration = 600.0f;

    [HideInInspector] public float ElapsedGameTime;
    [HideInInspector] public bool IsGameTime => ElapsedGameTime < GameDuration;
    [HideInInspector] public float RemainingGameTime => Mathf.Max(0.0f, GameDuration - ElapsedGameTime);


    [Header("Debug")]
    [SerializeField] private GameObject sphere;
    public GameObject Sphere => sphere;

    [SerializeField] private bool shouldShowDamageDumber;
    public bool ShouldShowDamageDumber => shouldShowDamageDumber;
    [SerializeField] private GameObject dmgNumber;

    private int killedEnemies = 0;
    public UnityEvent<int> OnEnemyKilled = new();
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

    public void ShowDamageNumber(Transform healthManagerTransform , int damage)
    {
        GameObject damageNumber = Instantiate(dmgNumber);
        Vector3 damageNumberPosition = healthManagerTransform.position;
        damageNumberPosition.y = PlayerBody.transform.position.y;

        Vector3 lookAtDir = damageNumber.transform.position- PlayerBody.transform.position ;
        lookAtDir = lookAtDir.normalized;

        damageNumber.transform.position = damageNumberPosition + Vector3.up * Random.Range(0.0f,1.0f) + lookAtDir * Random.Range(0.5f, 1.0f);
        if(damageNumber.TryGetComponent<DmgCount>(out DmgCount dmgCount))
        {
            dmgCount.ShowDamage(PlayerBody.transform,damage);
        }

    }

    public void EnemyDied()
    {
        killedEnemies++;
        OnEnemyKilled.Invoke(killedEnemies);
    }

    private void Update()
    {
        ElapsedGameTime += Time.deltaTime;
    }
}
