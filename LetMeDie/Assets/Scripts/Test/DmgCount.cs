using TMPro;
using UnityEngine;

public class DmgCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float removeTime = 0.5f;
    Transform player;
    public void ShowDamage(Transform player,int damage)
    {
        this.player = player;   
        text.text = damage.ToString();
        Destroy(gameObject, removeTime);
    }

    private void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }
}
