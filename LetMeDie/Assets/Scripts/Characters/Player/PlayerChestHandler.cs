using Essentials;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerChestHandler : MonoBehaviour
{

    [HideInInspector] public UnityEvent<int> OnCanOpenChest = new();
    [HideInInspector] public UnityEvent OnCanNotOpenChest = new();


    private bool canOpenChest;
    private ChestHandler currentChestHandler;
    private PlayerStatHandler playerStatHandler;

    private int amountOfOpenChests = 0;

    private void Start()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        PlayerInput actions = SInputManager.Instance.inputActions;
        actions.Keyboard.Interact.performed += OpenChest;
    }

    private void OpenChest(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OpenChest();
    }

    public void OpenChest()
    {
        if (currentChestHandler == null)
        {
            return;
        }
        int price = GetChestPrice(amountOfOpenChests);

        if (playerStatHandler.PlayerData.HasEnoughGold(price)) { 
            amountOfOpenChests++;
            playerStatHandler.PlayerData.RemoveGold(price);
            SUIManager.Instance.ChangeToUIState("GetItem");
            Destroy(currentChestHandler.gameObject);
        }
    }

    internal void CanNotOpenChest(ChestHandler chestHandler)
    {
        currentChestHandler = null;
        canOpenChest = false;
        OnCanNotOpenChest.Invoke();
    }

    private int GetChestPrice(int openedChests)
    {
        float basePrice = 30; 
        float increase = 1.10f;

        return Mathf.CeilToInt(basePrice * (float)Math.Pow(increase, openedChests));
    }

    internal void CanOpenChest(ChestHandler chestHandler)
    {
        currentChestHandler = chestHandler;
        canOpenChest = true;
        OnCanOpenChest.Invoke(GetChestPrice(amountOfOpenChests));
    }

}
