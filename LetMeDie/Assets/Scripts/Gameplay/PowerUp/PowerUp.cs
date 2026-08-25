using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;
    float rotation;
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void PickUp(GameObject player);
}
