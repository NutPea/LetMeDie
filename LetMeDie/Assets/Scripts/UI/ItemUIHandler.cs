using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIHandler : MonoBehaviour
{
    private ItemData currentItemData;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private LeanTweenType tweenType;
    [SerializeField] private float duration;
    [SerializeField] private float scaleAmount;

    private void Awake()
    {
        image.gameObject.SetActive(false);
    }

    public void Init(ItemData data)
    {
        if (data == null)
        {
            image.sprite = null;
            text.text = "";
            image.color = Color.white;
            image.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (currentItemData is ConsumbaleData consumbale)
        {
            consumbale.OnUse.RemoveListener(Use);
            consumbale.OnNoUsesLeft.RemoveListener(NoUsesLeft);
        }

        currentItemData = data;
        image.sprite = currentItemData.Sprite;
        image.color = data.Tint;


        if (currentItemData is ConsumbaleData currentConsumable)
        {
            HandleConsumable(currentConsumable);
        }else if(currentItemData is MagicSpell magic)
        {
            HandleMagicSpell(magic);
        }

        image.gameObject.SetActive(true);

    }

    private void HandleMagicSpell(MagicSpell magic)
    {
        text.text = magic.SpellManaCost.ToString();
        magic.OnSpellCast.AddListener(OnSpellCast);
    }

    private void OnSpellCast(MagicSpell magic)
    {
        LeanTween.cancel(gameObject);
        gameObject.transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), duration).setEase(tweenType).setOnComplete(ResetScale);
    }

    private void HandleConsumable(ConsumbaleData consumable)
    {
        text.text = consumable.Amount.ToString();
        consumable.OnUse.AddListener(Use);
        consumable.OnNoUsesLeft.AddListener(NoUsesLeft);
    }

    private void Use(ConsumbaleData consumbale)
    {
        LeanTween.cancel(gameObject);
        text.text = consumbale.Amount.ToString();
        gameObject.transform.localScale = Vector3.one;
        LeanTween.scale(gameObject,new Vector3(scaleAmount,scaleAmount,1),duration).setEase(tweenType).setOnComplete(ResetScale);
    }

    private void ResetScale()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), duration).setEase(tweenType);
    }

    private void NoUsesLeft()
    {
        image.gameObject.SetActive(false);
    }

}
