using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    PlayerCharacterControllerMovementController playerCharacterControllerMovement;
    PlayerStatHandler playerStatHandler;
    PlayerResourceHandler playerResourceHandler;

    void Start()
    {
        playerCharacterControllerMovement = GetComponent<PlayerCharacterControllerMovementController>();
        playerCharacterControllerMovement.OnStartJump.AddListener(PlayStartJumpSound);
        playerCharacterControllerMovement.OnLandAfterJump.AddListener(PlayLandAfterJumpSound);

        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerStatHandler.PlayerData.OnLevelUp.AddListener(PlayLevelUpSound);

        playerResourceHandler = GetComponent<PlayerResourceHandler>();
        playerResourceHandler.OnDamaged.AddListener(PlayDamage);
        playerResourceHandler.OnHeal.AddListener(PlayHeal);
    }

    private void PlayHeal()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_Heal);
    }

    private void PlayDamage(bool arg0, int arg1, Transform arg2)
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_Hit);
    }

    private void PlayLevelUpSound(int arg0)
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Feedback_Levelup);
    }

    private void Update()
    {
        
    }

    private void PlayDashStartSound()
    {
        return;
       // SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Player_Dash_Start);
    }

    private void PlayDashStopSound()
    {
        return;
       // SoundManager.instance.PlayRandomLibarySound(SoundLibary.SFX.Player_Dash_Stop);
    }

    private void PlayWallBounceSound()
    {
        return;
        //SoundManager.instance.PlayRandomLibarySound(SoundLibary.SFX.Player_Default_Wall_Hit);
    }


    private void PlaySlideSound()
    {
        return;
        //SoundManager.instance.PlayRandomContinuousLibarySound(SoundLibary.SFX.Player_Default_Slide);
    }

    private void PlayCrouchSound()
    {
        return;
       // SoundManager.instance.PlayRandomContinuousLibarySound(SoundLibary.SFX.Player_Default_Crouch);
    }

    private void PlayLandAfterJumpSound()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Character_Land);
    }

    private void PlayStartJumpSound()
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Character_Jump);
    }


}
