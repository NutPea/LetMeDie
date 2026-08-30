using UnityEngine;
using UnityEngine.Events;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Rigidbody rb;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    private bool canMove = true;
    public bool CanMove => canMove;
    [SerializeField] private bool isThrowed;
    private int damage;
    public int Damage => damage;
    private TeamFlag team;

    private float currentChargeAmount;
    public float CurrentChargeAmount => currentChargeAmount;
    [HideInInspector] public UnityEvent OnCollided = new();
    [HideInInspector] public UnityEvent OnDestroyAfterDurability = new();
    [SerializeField] private bool destroyOnInpact = true;
    [SerializeField] private bool isAOE;
    [SerializeField] private bool isPiercing;
    [SerializeField] private bool stopWhenHit;
    [SerializeField] private float aoeRange = 2f;
    [SerializeField] private bool hasDurability;
    [SerializeField] private float durability = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(int damage , float chargeAmount,Vector3 lookDirection,TeamFlag team)
    {
        if (hasDurability)
        {
            Invoke(nameof(RemoveAfterDurability), durability);
        }
        else
        {
            Destroy(gameObject,10f);
        }
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, chargeAmount);
        transform.forward = lookDirection;
        canMove = true;
        this.damage = damage;
        this.team = team;
        currentChargeAmount = chargeAmount;
        if (isThrowed)
        {

            rb.AddForce(transform.forward * currentSpeed,ForceMode.Impulse);
        }
    }

    private void RemoveAfterDurability()
    {
        OnDestroyAfterDurability.Invoke();
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isThrowed)
        {
            return;
        }
        if (!canMove) {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            return;
        }
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isThrowed)
        {
            return;
        }
        GameObject hittedObject = null;
        if (other.gameObject.layer == LayerMask.NameToLayer("Hitable"))
        {
            hittedObject = other.gameObject;
            other.gameObject.GetComponent<HealthManager>().InflictDamage(damage, team, transform);
            if (!isPiercing)
            {
                if (!hasDurability)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!hasDurability)
                {
                    Destroy(gameObject, 1f);
                }
            }
        }
        else if (destroyOnInpact)
        {
            if (!hasDurability)
            {
                Destroy(gameObject);
                OnCollided.Invoke();
            }
        }
        if (isAOE)
        {
            Collider[] hittable = Physics.OverlapSphere(transform.position, aoeRange, layerMask);
            foreach(Collider col in hittable)
            {
                if(col.gameObject == hittedObject)
                {
                    continue;
                }
                col.gameObject.GetComponent<HealthManager>().InflictDamage(damage, team, transform);
            }
        }

        if (stopWhenHit)
        {
            canMove = false;
        }
    }

}
