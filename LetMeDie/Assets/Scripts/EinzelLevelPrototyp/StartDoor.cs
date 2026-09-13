using UnityEngine;
using UnityEngine.Events;

public class StartDoor : MonoBehaviour
{
    [SerializeField] private Transform playerStartTransform;
    public Transform PlayerStart => playerStartTransform;

    public UnityEvent OnPlayerIsReady = new UnityEvent();

    private void Start()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        SGameManager.Instance.PlayerBody.transform.position = playerStartTransform.position;
        SGameManager.Instance.PlayerBody.transform.forward = playerStartTransform.forward;
        //Do intro
        OnPlayerIsReady.Invoke();
    }


}
