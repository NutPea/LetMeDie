using UnityEngine;

[RequireComponent (typeof(BaseEnemyController))]
public class AggroRadiusExtension : MonoBehaviour
{
    [SerializeField] private float aggroRange = 5f;
    [SerializeField] private float aggroAngle = 90f;
    [SerializeField] private float checkTime = 1f;

    private LayerMask layerMask;

    private BaseEnemyController baseEnemyController;
    private Collider[] results;
    private int maxCollider = 3;
    private float maxRange = 100f;
    private void Start()
    {
        baseEnemyController = GetComponent<BaseEnemyController>();
        Invoke(nameof(CheckForPlayer),checkTime);
        layerMask = LayerMask.GetMask("Player");


    }

    private void CheckForPlayer()
    {
        if (baseEnemyController.IsAggro) { 
            return;
        }

        results = new Collider[maxCollider];
        Physics.OverlapSphereNonAlloc(transform.position, aggroRange,results, layerMask);
        if (results.Length > 0)
        {
            foreach (Collider collider in results) {
                if(collider == null) continue;
                if (collider.CompareTag("Player"))
                {
                    Vector3 direction = collider.transform.position - transform.position;
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position,direction,out hit,maxRange, ~0))
                    {
                        if (!hit.collider.CompareTag("Player")){
                            break;
                        }
                        if(Vector3.Angle(transform.forward,direction) < aggroAngle){
                            baseEnemyController.SetAggro();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        Invoke(nameof(CheckForPlayer), checkTime);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Vector3 rotatedVector1 = Quaternion.AngleAxis(-aggroAngle, Vector3.up) * transform.forward;
        Vector3 rotatedVector2 = Quaternion.AngleAxis(aggroAngle, Vector3.up) * transform.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position+rotatedVector1 * aggroRange);
        Gizmos.DrawLine(transform.position, transform.position + rotatedVector2 * aggroRange);
    }
}
