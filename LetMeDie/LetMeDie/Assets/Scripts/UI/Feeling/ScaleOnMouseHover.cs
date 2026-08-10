using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnMouseHover : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler  , ISelectHandler , IDeselectHandler{


    [SerializeField] private Vector2 newScale;
    [SerializeField] private float scaleTime = 0.1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeInSine;


    public void OnPointerEnter(PointerEventData eventData) {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(newScale.x,newScale.y,1), scaleTime).setEase(tweenType).setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, scaleTime).setEase(tweenType).setIgnoreTimeScale(true);
    }

    public void OnSelect(BaseEventData eventData) {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(newScale.x, newScale.y, 1), scaleTime).setEase(tweenType).setIgnoreTimeScale(true);
    }
    public void OnDeselect(BaseEventData eventData) {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, scaleTime).setEase(tweenType).setIgnoreTimeScale(true);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }
}
