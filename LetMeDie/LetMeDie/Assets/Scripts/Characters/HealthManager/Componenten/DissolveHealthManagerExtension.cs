using System;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class DissolveHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;

    [SerializeField] private float _dissolveDelay = 1;
    [SerializeField] private float _dissolveTime = 1;
    [SerializeField] private LeanTweenType _dissolveType = LeanTweenType.linear;
    [ColorUsage(true, true)]
    [SerializeField] private Color startDissolveColor = Color.white;
    [ColorUsage(true, true)]
    [SerializeField] private Color endDissolveColor = Color.blue;

    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private const string MAIN_TEX_STRING = "_MainTex";
    public const string MAIN_TEX_COLOR_STRING = "_Color";
    private const string DISSOLVE_MAIN_TEX_TINT_STRING = "_MainTexTint";
    private const string TEXTURE_TINT_STRING = "_Tint";
    private const string NORMAL_TEX_STRING = "_Normal";
    private const string DISSOLVE_STRING = "_DissolveAmount";

    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(GameObject diedObject)
    {
        Invoke(nameof(Dissolve), _dissolveDelay);
    }

    private void Dissolve()
    {
        Debug.Log("StartDissolve");
        Material baseMaterial = skinnedMeshRenderer.material;
        Material tempDissolveMaterial = new Material(dissolveMaterial);
        tempDissolveMaterial.SetFloat(DISSOLVE_STRING, 0);
        tempDissolveMaterial.SetTexture(MAIN_TEX_STRING, baseMaterial.GetTexture(MAIN_TEX_STRING));

        skinnedMeshRenderer.material = tempDissolveMaterial;

        LeanTween.value(gameObject, 0, 1, _dissolveTime).setOnUpdate((float val) =>
        {
            SetDissolveAmount(tempDissolveMaterial,val);
            SetDissolveColor(tempDissolveMaterial,val);
        }).setEase(_dissolveType).setOnComplete(Destroy);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetDissolveColor(Material dissolve ,float amount)
    {
        Color lerpedColor = Color.Lerp(startDissolveColor, endDissolveColor, amount);
        dissolve.SetColor(TEXTURE_TINT_STRING, lerpedColor);
    }

    private void SetDissolveAmount(Material dissolve ,float amount)
    {
        dissolve.SetFloat(DISSOLVE_STRING, amount);
    }



}
