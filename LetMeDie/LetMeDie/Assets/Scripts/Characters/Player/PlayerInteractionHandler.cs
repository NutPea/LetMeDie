using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionHandler : MonoBehaviour
{

    public LayerMask interactableLayerMask;
    public float interactionDistance = 1f;
    private Camera mainCam;
    PlayerInput inputActions;
    bool seeInteractable;
    public UnityEvent OnCanBeInteracted = new UnityEvent();
    public UnityEvent OnCanNotBeInteractedAnymore = new UnityEvent();
    public UnityEvent OnInteract = new UnityEvent();

    void Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.Interact.started += HandleInteraction;
        mainCam = Camera.main;
    }

    private void HandleInteraction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            IInteractable[] allInteractables = hit.transform.gameObject.GetComponents<IInteractable>();
            foreach (IInteractable interactable in allInteractables)
            {
                interactable.OnInteract(transform);
            }
            OnInteract.Invoke();
        }
    }

    private void Update()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, interactionDistance, interactableLayerMask))
        {
            if(!seeInteractable)
            {
                OnCanBeInteracted.Invoke();
                seeInteractable = true;
            }
        }
        else
        {
            if (seeInteractable)
            {
                OnCanNotBeInteractedAnymore.Invoke();
                seeInteractable = false;
            }
        }
    }


}
