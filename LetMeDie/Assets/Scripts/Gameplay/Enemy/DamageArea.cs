using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform fillArea;
    private int damage;
    private float timeUntilExplosion;
    private float currentTimeUntilExplosion;
    private bool startDamageArea = false;
    private float size;

    public void StartDamageArea(float size,float timeUntilExplodes,int damage)
    {
        transform.localScale = new Vector3(size, size, size); 
        this.damage = damage;
        this.timeUntilExplosion = timeUntilExplodes;
        startDamageArea = true;
        currentTimeUntilExplosion = timeUntilExplosion;
        this.size = size;
    }

    private void Update()
    {
        if (!startDamageArea)
        {
            return;
        }

        if(currentTimeUntilExplosion < 0)
        {
            Damage();
            startDamageArea = false;
            Destroy(gameObject);
        }
        else
        {
            currentTimeUntilExplosion -= Time.deltaTime;
            float fill = 1 - currentTimeUntilExplosion / timeUntilExplosion;
            fillArea.localScale = new Vector3(fill, fill, fill);
        }
    }

    private void Damage()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, size, playerLayerMask);
        foreach (Collider collider in player) {
            if(collider.gameObject.TryGetComponent(out PlayerResourceHandler playerResourceHandler))
            {
                playerResourceHandler.InflictDamage(damage, TeamFlag.Enemy, transform);
            }
        }
    }
}
