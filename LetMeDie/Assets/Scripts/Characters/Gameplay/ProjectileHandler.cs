using UnityEngine;
using UnityEngine.Events;

public class ProjectileHandler : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    private bool canMove = true;
    public bool CanMove => canMove;
    private int damage;
    private TeamFlag team;

    private float currentChargeAmount;
    public float CurrentChargeAmount => currentChargeAmount;
    [HideInInspector] public UnityEvent OnCollided = new();
    [SerializeField] private bool destroyOnInpact = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(int damage , float chargeAmount,Vector3 lookDirection,TeamFlag team)
    {
        Destroy(gameObject,10f);
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, chargeAmount);
        transform.forward = lookDirection;
        canMove = true;
        this.damage = damage;
        this.team = team;
        currentChargeAmount = chargeAmount;
    }

    private void FixedUpdate()
    {
        if (!canMove) {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            return;
        }
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCollided.Invoke();
        if (other.gameObject.layer == LayerMask.NameToLayer("Hitable"))
        {
            other.gameObject.GetComponent<HealthManager>().InflictDamage(damage, team, transform);
            Destroy(gameObject);
        }
        if (destroyOnInpact)
        {
            Destroy(gameObject);
        }
        canMove = false;
    }

}
