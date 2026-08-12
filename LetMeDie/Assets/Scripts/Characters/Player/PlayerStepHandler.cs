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

    PlayerCharacterControllerMovementController playerCharacterControllerMovement;
    public StepUnderground currentUnderground;



    void Start()
    {

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
