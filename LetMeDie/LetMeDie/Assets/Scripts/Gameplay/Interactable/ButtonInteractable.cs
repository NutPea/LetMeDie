using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour , IInteractable
{
    public UnityEvent OnInteractEvent;

    [Header("Animation")]
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private float moveAmount = 2f;
    [SerializeField] private float moveTime = 0.2f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeOutBack;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = buttonObject.transform.position;
    }
    public void OnInteract(Transform player)
    {
        OnInteractEvent.Invoke();
        LeanTween.cancel(buttonObject);
        buttonObject.transform.position = startPosition;
        LeanTween.move(buttonObject,buttonObject.transform.position + -buttonObject.transform.up * moveAmount,moveTime).setEase(tweenType).setOnComplete(Reset);
    }

    private void Reset()
    {
        LeanTween.move(buttonObject, startPosition, moveTime).setEase(tweenType);
    }


}
