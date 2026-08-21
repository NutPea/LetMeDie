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
    private int damage;
    private TeamFlag team;

    private float currentChargeAmount;
    public float CurrentChargeAmount => currentChargeAmount;
    [HideInInspector] public UnityEvent OnCollided = new();
    [SerializeField] private bool destroyOnInpact = true;
    [SerializeField] private bool isAOE;
    [SerializeField] private bool isPiercing;
    [SerializeField] private bool stopWhenHit;
    [SerializeField] private float aoeRange = 2f;

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
        GameObject hittedObject = null;
        if (other.gameObject.layer == LayerMask.NameToLayer("Hitable"))
        {
            hittedObject = other.gameObject;
            other.gameObject.GetComponent<HealthManager>().InflictDamage(damage, team, transform);
            if (!isPiercing)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 1f);
            }
        }
        else if (destroyOnInpact)
        {
            Destroy(gameObject);
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
                Debug.Log(col.name);
                col.gameObject.GetComponent<HealthManager>().InflictDamage(damage, team, transform);
            }
        }

        if (stopWhenHit)
        {
            canMove = false;
        }
    }

}
