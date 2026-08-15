using System;
using UnityEngine;

public class CheatHandler : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;


    void Start()
    {
        PlayerInput inputs = SInputManager.Instance.inputActions;
        inputs.Cheats.LevelUp.performed += LevelUpPlayer;

        playerStatHandler = GetComponent<PlayerStatHandler>();
    }

    private void LevelUpPlayer(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        playerStatHandler.PlayerData.ForceLevelUp();
    }

}
