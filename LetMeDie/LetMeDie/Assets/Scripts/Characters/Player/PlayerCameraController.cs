using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    PlayerInput inputActions;
    public float cameraPitch = 0.0f;
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform cameraRoot;
    private DialogueSystemEvents dialogueSystemEvents;

    // Start is called before the first frame update

    private Transform lookAtTransform;
    private bool shouldLookAt = false;
    [SerializeField] private float lookAtSpeed = 2f;

    private IEnumerator Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.MouseMovement.performed += ctx => MouseMovement();
        dialogueSystemEvents = GetComponent<DialogueSystemEvents>();
        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener(StopLookAt);

        yield return new WaitForSeconds(0.5f);

    }
    private void MouseMovement()
    {
        if (SGameManager.IsPaused) return;
        if (Time.timeScale <= 0) return;
        if (shouldLookAt) return;
        /*
        if (PauseUiHandler.isPaused) return;
        if (TutorialUI.inTutorial) return;
        if (TimeHandler.isInTimeHandler) return;
        */
        Vector2 mouseDelta = inputActions.Keyboard.MouseMovement.ReadValue<Vector2>();

        cameraPitch -= mouseDelta.y * sensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90f);

        playerCameraTransform.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);
    }

    public void LookAtTransform(Transform lookAtTransform)
    {
        this.lookAtTransform = lookAtTransform;
        shouldLookAt = true;
    }

    private void StopLookAt(Transform transform)
    {
        float localEulerValue = playerCameraTransform.localEulerAngles.x;
        if (localEulerValue > 180)
        {
            cameraPitch = localEulerValue - 360f;
        }
        else
        {
            cameraPitch = localEulerValue;
        }
        shouldLookAt = false;
    }

    private void Update()
    {
        if (!shouldLookAt) { return; }
        Vector3 lookAtDirection = lookAtTransform.transform.position - transform.position;
        lookAtDirection = lookAtDirection.normalized;
        lookAtDirection.y = 0;


        // Lerp
        transform.forward = lookAtDirection;
        playerCameraTransform.LookAt(lookAtTransform);
    }

}
