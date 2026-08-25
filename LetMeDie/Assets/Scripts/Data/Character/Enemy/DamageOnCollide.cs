using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{

    [SerializeField] private int amountOfDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Damage(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage(collision.gameObject);
        }
    }

    private void Damage(GameObject player)
    {
        player.GetComponent<HealthManager>().InflictDamage(amountOfDamage,TeamFlag.Enemy,transform);
    }

}
