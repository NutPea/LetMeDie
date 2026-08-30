using UnityEngine;
[CreateAssetMenu(fileName = "SpreadMagicProjectileSpell", menuName = "Weapons/Magic/SpreadMagicProjectileSpell", order = 1)]
public class SpreadMagicProjectileSpell : MagicProjectileSpell
{
    [Header("Spread")]
    [SerializeField] private int amountOfProjektiles = 1;
    [SerializeField] private Vector2 spreadRadius;
    [SerializeField] private bool spreadIsRandom = false;

    public override void Cast(Transform camera)
    {
        base.Cast(camera);
       
        if(amountOfProjektiles > 1)
        {
            int amountOfExtraShots = amountOfProjektiles - 1;
            if (spreadIsRandom)
            {
                for(int i = 0; i< amountOfExtraShots; i++)
                {
                    Vector3 newProjectileLookDirection = camera.transform.right * Random.Range(-spreadRadius.x, spreadRadius.x) + camera.transform.up * Random.Range(-spreadRadius.y, spreadRadius.y) + camera.transform.forward;
                    newProjectileLookDirection = newProjectileLookDirection.normalized;
                    SpawnProjectile(camera, camera.transform.position, newProjectileLookDirection);
                }
            }
            else
            {
                int rightAmount = Mathf.CeilToInt((float)amountOfExtraShots / 2);
                int leftAmount = amountOfExtraShots - rightAmount;

                Debug.Log(rightAmount);
                Debug.Log(leftAmount);

                float rightStep = spreadRadius.x / rightAmount;
                for (int x = 0; x < rightAmount; x++)
                {
                    Vector3 newProjectileLookDirection = camera.transform.right * rightStep * (x+1) + camera.transform.forward;
                    newProjectileLookDirection = newProjectileLookDirection.normalized;
                    SpawnProjectile(camera, camera.transform.position, newProjectileLookDirection);
                }

                for (int y = 0; y < leftAmount; y++)
                {
                    Vector3 newProjectileLookDirection = camera.transform.right * -rightStep * (y+1) + camera.transform.forward;
                    newProjectileLookDirection = newProjectileLookDirection.normalized;
                    SpawnProjectile(camera, camera.transform.position, newProjectileLookDirection);
                }
            }
        }
        
    }


}
