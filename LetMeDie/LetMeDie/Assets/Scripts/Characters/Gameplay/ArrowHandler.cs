using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(ProjectileHandler))]
public class ArrowHandler : MonoBehaviour
{
    private ProjectileHandler projectileHandler;
    [SerializeField] private float minTippingSpeed;
    [SerializeField] private float maxTippingSpeed;

    private float CurrentTippingSpeed => Mathf.Lerp(minTippingSpeed, maxTippingSpeed, projectileHandler.CurrentChargeAmount);

    void Start()
    {
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!projectileHandler.CanMove)
        {
            return;
        }
        transform.Rotate(Vector3.right * CurrentTippingSpeed * Time.deltaTime);
    }
}
