using UnityEngine;

public class ExpSphere : MonoBehaviour
{

    [SerializeField] private int experienceAmount = 500;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler statHandler = other.GetComponent<PlayerStatHandler>();
            if (statHandler != null) {
                statHandler.PlayerData.AddExperience(experienceAmount);
            }

        }
    }
}
