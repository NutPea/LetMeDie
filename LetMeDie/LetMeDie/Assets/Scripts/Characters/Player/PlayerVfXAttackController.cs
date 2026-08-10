 using UnityEngine;
using UnityEngine.VFX;

public class PlayerVfXAttackController : MonoBehaviour
{
    [SerializeField] private WeaponTrailHandler weaponTrailHandler;
    private WeaponData weaponData;
    private SwordData swordData;

    [SerializeField] private GameObject hitBox;
    [SerializeField] private VisualEffect rightSwingVFX;
    [SerializeField] private VisualEffect leftSwingVFX;

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

    public void SetSwordWeapon(WeaponHandler weaponHandler,SwordData data)
    {
        weaponTrailHandler.Tip = weaponHandler.Tip;
        weaponTrailHandler.Base = weaponHandler.Base;
        weaponTrailHandler.SetColor(data.TrailColor);
    }

    public void PlaySwordAttackVFXRight()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Sword_Swing_Default);
        rightSwingVFX.Play();

    }

    public void PlaySwordAttackVFXLeft()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Sword_Swing_Default);
        leftSwingVFX.Play();
    }

    public void ShowTrail()
    {
      //  weaponTrailHandler.ShowTrail();
    }

    public void HideTrail() {
      //  weaponTrailHandler.HideTrail();
    }
}
