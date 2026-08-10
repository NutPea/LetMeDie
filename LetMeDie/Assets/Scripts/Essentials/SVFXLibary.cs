using UnityEngine;

public class SVFXLibary : MonoBehaviour
{

    public static SVFXLibary Instance; 
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum VFXNames
    {
        StandartHit = 0,
        GroundHit = 1,
    }

    [SerializeField]private GameObject StandartHit;
    [SerializeField]private GameObject GroundHit;

    public GameObject GetVFX(VFXNames vfx)
    {
        GameObject foundVfx = null;
        switch (vfx)
        {
            case VFXNames.StandartHit: foundVfx = StandartHit; break;
            case VFXNames.GroundHit: foundVfx = GroundHit; break;
        }

        return foundVfx;
    }
}
