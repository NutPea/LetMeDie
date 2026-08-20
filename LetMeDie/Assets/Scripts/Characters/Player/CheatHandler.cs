using Essentials;
using System;
using UnityEngine;

public class CheatHandler : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;


    void Start()
    {
        PlayerInput inputs = SInputManager.Instance.inputActions;
        inputs.Cheats.LevelUp.performed += LevelUpPlayer;
        inputs.Cheats.GetItem.performed += GetItem;
        inputs.Cheats.GetSpell.performed += GetSpell;

        playerStatHandler = GetComponent<PlayerStatHandler>();
    }

    private void LevelUpPlayer(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        playerStatHandler.PlayerData.ForceLevelUp();
    }


    private void GetItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SUIManager.Instance.ChangeToUIState("GetItem");
    }

    private void GetSpell(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SUIManager.Instance.ChangeToUIState("SpellLevelUp");
    }
}
