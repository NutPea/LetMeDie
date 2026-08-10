
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class FlashOnHitHealthManagerExtension : MonoBehaviour
{
    HealthManager _healthManager;
    public float timeBetweenFlashes = 0.1f;
    public Material flashMaterial;
    Material rootMaterial;

    public List<MeshRenderer> meshRenderers;
    private Dictionary<MeshRenderer,Material> meshRenderMaterials = new();
    public List<SkinnedMeshRenderer> skinnedsMeshRenderers;
    private Dictionary<SkinnedMeshRenderer,Material> skinnedMeshRenderMaterials = new();


    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
         foreach(MeshRenderer meshRenderer in meshRenderers)
         {
            meshRenderMaterials.Add(meshRenderer, meshRenderer.material);
         }

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedsMeshRenderers)
        {
            skinnedMeshRenderMaterials.Add(skinnedMeshRenderer, skinnedMeshRenderer.material);
        }
        _healthManager.OnDamaged.AddListener(OnDamageTaken);
    }


    void OnDamageTaken(bool isDead, int damage,Transform hitpos)
    {
        StartCoroutine(OnFlash());
    }

    IEnumerator OnFlash()
    {
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetRootColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetRootColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetRootColor();
    }



    void SetRootColor()
    {
        foreach(KeyValuePair<MeshRenderer,Material> valuePair in meshRenderMaterials)
        {
            List<Material> matList = new();
            matList.Add(valuePair.Value);
            valuePair.Key.materials = matList.ToArray();
        }

        foreach (KeyValuePair<SkinnedMeshRenderer, Material> valuePair in skinnedMeshRenderMaterials)
        {
            List<Material> matList = new();
            matList.Add(valuePair.Value);
            valuePair.Key.materials = matList.ToArray();
        }
    }

    void SetFlashColor()
    {
        List<Material> flashList = new();
        flashList.Add(flashMaterial);
        foreach (KeyValuePair<MeshRenderer, Material> valuePair in meshRenderMaterials)
        {
            valuePair.Key.materials = flashList.ToArray();
        }

        foreach (KeyValuePair<SkinnedMeshRenderer, Material> valuePair in skinnedMeshRenderMaterials)
        {
            valuePair.Key.materials = flashList.ToArray();
        }
    }

}
