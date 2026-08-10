using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPickUpHandler : MonoBehaviour
{

    public float interactionDistance = 1f;
    private Camera mainCam;
    PlayerInput inputActions;
    bool hasPickUp;
    bool seePickUpable;
    public UnityEvent OnCanBePickedUp = new UnityEvent();
    public UnityEvent OnCanNotBePickedUpAnymore = new UnityEvent();
    public UnityEvent<PickupHandler> OnPickUp = new ();
    public UnityEvent OnDrop = new();

    private PickupHandler currentPickUpHandler;

    void Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.Interact.started += HandlePickUp;
        inputActions.Keyboard.Interact.canceled += HandleDrop;


        mainCam = Camera.main;
    }

    private void HandleDrop(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (hasPickUp)
        {
            OnDrop.Invoke();
            currentPickUpHandler = null;
            hasPickUp = false;
        }
    }

    private void HandlePickUp(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (hasPickUp) {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, interactionDistance, ~0))
        {
            PickupHandler pickupHandler = hit.transform.gameObject.GetComponent<PickupHandler>();
            currentPickUpHandler = pickupHandler;
            OnPickUp.Invoke(currentPickUpHandler);
            hasPickUp = true;
        }
    }

    private void Update()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, interactionDistance, ~0))
        {
            if (!seePickUpable)
            {
                OnCanBePickedUp.Invoke();
                seePickUpable = true;
            }
        }
        else
        {
            if (seePickUpable && !hasPickUp)
            {
                OnCanNotBePickedUpAnymore.Invoke();
                seePickUpable = false;
            }
        }
    }
}
