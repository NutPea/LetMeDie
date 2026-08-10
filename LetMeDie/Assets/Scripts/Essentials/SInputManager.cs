using UnityEngine;

public class SInputManager : MonoBehaviour
{

    private static SInputManager _instance;
    public static SInputManager Instance => _instance;

    public PlayerInput inputActions;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            transform.parent = null;
           // DontDestroyOnLoad(gameObject);
            inputActions = new();
        }
        else
        {
            Destroy(gameObject);
        }

    }


}
