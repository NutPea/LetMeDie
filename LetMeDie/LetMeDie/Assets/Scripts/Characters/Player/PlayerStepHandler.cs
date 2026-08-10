using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepHandler : MonoBehaviour
{
    public enum StepUnderground { 
        Default = 0,
        Water = 1,
        Iron = 2,
        HollowIron = 3
    }

    PlayerMovementController playerMovementController;
    public StepUnderground currentUnderground;



    void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        playerMovementController.OnIsWalking.AddListener(PlayWalkSound);
        playerMovementController.OnIsSprinting.AddListener(PlaySprintSound);
    }

    private void PlaySprintSound()
    {
        return;
    }

    private void PlayWalkSound()
    {
        SoundManager.instance.PlayContinuousLibarySound(SoundLibary.SFX.Character_Walk_Default);
    }


    public void SetUnderground(StepUnderground underground) {
        currentUnderground = underground;
    }

    public void ResetUnderground()
    {
        currentUnderground = StepUnderground.Default;
    }

}
