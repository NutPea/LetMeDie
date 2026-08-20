using UnityEngine;
using UnityEngine.UI;

public class CrossHairUIHandler : MonoBehaviour
{

    [SerializeField] private Sprite defaulWeapontSprite;
    [SerializeField] private Sprite rangeWeaponSprite;

    [SerializeField] private Sprite interactionSprite;
    [SerializeField] private Sprite pickUpSprite;

    [SerializeField] private Image crossairImage;
    [SerializeField] private Color minChargeColor;
    [SerializeField] private Color maxChargeColor;

    [SerializeField] private float minChargeScaleAmount;
    [SerializeField] private float maxChargeScaleAmount;

    [SerializeField] private Color maxColor;
    private bool isMaxedOut;

    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeInQuint;
    [SerializeField] private float scaleTime = 0.05f;
    [SerializeField] private float scaleAmount = 1.1f;

    private UnityEngine.Sprite previouseSprite;
    private bool canNotChangeAlphaValue = false;
    private float beforeAlphaValue = 0f;

    private void Awake()
    {
        crossairImage = GetComponent<Image>();
        crossairImage.transform.localScale = new Vector3(minChargeScaleAmount, minChargeScaleAmount, 1);
    }

    public void ChangeToInteractionSprite()
    {
        ChangeTemporarySprite(interactionSprite);
    }

    public void ChangeToPickUpSprite()
    {
        ChangeTemporarySprite(pickUpSprite);
    }

    private void ChangeTemporarySprite(UnityEngine.Sprite sprite)
    {
        previouseSprite = crossairImage.sprite;
        crossairImage.sprite = sprite;

        Color crosshair = crossairImage.color;
        beforeAlphaValue = crosshair.a;
        crosshair.a = 1;
        crossairImage.color = crosshair;
        canNotChangeAlphaValue = true;
    }

    public void ReturnToPreviouseSprite()
    {
        Color crosshair = crossairImage.color;
        crosshair.a = beforeAlphaValue;
        crossairImage.color = crosshair;

        canNotChangeAlphaValue = false;
        crossairImage.sprite = previouseSprite;

    }

    public void ChangeSprite(WeaponData weaponData)
    {
        if (weaponData.GetType() == typeof(BowData))
        {
            crossairImage.sprite = rangeWeaponSprite;
        }
        else
        {
            crossairImage.sprite = defaulWeapontSprite;
        }
    }

    public void SetValue(float percentage)
    {
        if (isMaxedOut || canNotChangeAlphaValue)
        {
            return;
        }
        crossairImage.color = Color.Lerp(minChargeColor, maxChargeColor, percentage);
        float percentScaleAmount = Mathf.Lerp(minChargeScaleAmount, maxChargeScaleAmount, percentage);
        crossairImage.transform.localScale = new Vector3(percentScaleAmount, percentScaleAmount, 1);
        if(percentage >= 1)
        {
            LeanTween.scale(crossairImage.gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setOnComplete(ScaleBack).setEase(tweenType);
            crossairImage.color = maxColor;
            isMaxedOut = true;
        }
    }

    private void ScaleBack()
    {
        LeanTween.scale(crossairImage.gameObject, new Vector3(1, 1, 1), scaleTime).setEase(tweenType);
    }

    public void ResetValue()
    {
        crossairImage.color = minChargeColor;
        crossairImage.transform.localScale = new Vector3(minChargeScaleAmount, minChargeScaleAmount, 1);
        isMaxedOut = false;
    }





}
