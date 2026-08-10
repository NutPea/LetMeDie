using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerParticleHandler : MonoBehaviour
{
    public float speedPartikelShowMagnitude = 6.3f;
    public float speedPartikelHideMagnitude = 4f;
    private Rigidbody rb;
    private PlayerMovementController playerMovementController;
    public ParticleSystem sprintPartikle;
    private bool showSpeedPartikelTrigger = false;
    private bool hideSpeedPartikelTrigger = false;
    [HideInInspector] public UnityEvent ShowSpeedPartikel;
    [HideInInspector] public UnityEvent HideSpeedPartikel;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovementController = GetComponent<PlayerMovementController>();
        showSpeedPartikelTrigger = false;
        hideSpeedPartikelTrigger = true;

        sprintPartikle.Stop();
        ShowSpeedPartikel.AddListener(() => sprintPartikle.Play());
        HideSpeedPartikel.AddListener(() => sprintPartikle.Stop());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementController.isGrounded)
        {
            if(rb.linearVelocity.magnitude >= speedPartikelShowMagnitude && 
                playerMovementController.currentMovementState == PlayerMovementController.MovementState.Sprinting)
            {
                if (!showSpeedPartikelTrigger)
                {
                    ShowSpeedPartikel.Invoke();
                    showSpeedPartikelTrigger = true;
                    hideSpeedPartikelTrigger = false;
                }
            }
            else if(rb.linearVelocity.magnitude <= speedPartikelHideMagnitude)
            {
                if (!hideSpeedPartikelTrigger)
                {
                    HideSpeedPartikel.Invoke();
                    showSpeedPartikelTrigger = false;
                    hideSpeedPartikelTrigger = true;
                }
            }
        }
    }
}
