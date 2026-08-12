 using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimationAttackController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController playerWeaponController;
    private WeaponData weaponData;
    private SwordData swordData;

    [SerializeField] private GameObject hitBox;
    [SerializeField] private VisualEffect leftSwordVisuellEffect;
    [SerializeField] private VisualEffect rightSwordVisuellEffect;

    public void SetWeaponData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        if(weaponData is SwordData data)
        {
            this.swordData = data;
        }
    }

    public void ShowSwordHitVFX()
    {
        swordData.PlayHitVFX();
        if (swordData.ShowAttackHitbox)
        {
            Transform mainCam = SCameraShake.Instance.CurrentlyUsedCamera.transform ;
            GameObject box = Instantiate(hitBox, mainCam.transform.position + mainCam.transform.forward * swordData.Range, mainCam.rotation);
            box.transform.localScale = swordData.GetHalfBoxExtend(mainCam) * 2;
            Destroy(box,2f);
        }
    }

    public void Attack()
    {
        playerWeaponController.AnimationAttack();
    }


    public void PlaySwordWoosh()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Sword_Swing_Default);
    }

    public void ShowTrail()
    {
        //weaponTrailHandler.ShowTrail();
    }

    public void HideTrail() {
       // weaponTrailHandler.HideTrail();
    }
}
