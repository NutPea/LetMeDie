using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
   
    public UnityEvent OnButtonClicked;


    [SerializeField] private GameObject toMoveButton;
    [SerializeField] private float pressInTime;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeInOutBack;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = toMoveButton.transform.position; 
    }
    public void OnInteract(Transform player)
    {
        OnButtonClicked.Invoke();
        LeanTween.move(toMoveButton,toMoveButton.transform.position -transform.up * 0.5f,pressInTime).setEase(tweenType).setOnComplete(MoveBack);
    }

    private void MoveBack()
    {
        LeanTween.move(toMoveButton, startPosition, pressInTime).setEase(tweenType);
    }

}
