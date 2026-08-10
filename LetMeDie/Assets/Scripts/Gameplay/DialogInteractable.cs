using Essentials;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

[RequireComponent (typeof(DialogueSystemTrigger))]
[RequireComponent(typeof(DialogueSystemEvents))]
public class DialogInteractable : MonoBehaviour , IInteractable
{
    private DialogueSystemTrigger dialogueSystemTrigger;
    private PlayerCameraController playerCameraController;
    [SerializeField] private Transform dialogLookAtTransform;

    private void Start()
    {
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
    }


    public void OnInteract(Transform player)
    {
        if (DialogueManager.instance.activeConversation == null)
        {
            if(playerCameraController == null)
            {
                playerCameraController = player.GetComponent<PlayerCameraController>();
            }

            playerCameraController.LookAtTransform(dialogLookAtTransform);
            SUIManager.Instance.ChangeToUIState("Dialog");
            dialogueSystemTrigger.TryStart(player);
        }
    }


}
