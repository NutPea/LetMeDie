using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerData playerStartData;

    [HideInInspector] public UnityEvent<PlayerData> OnStatUpdate = new();
    [SerializeField] private PlayerData _clonedPlayerData;
    public PlayerData PlayerData
    {
        get {
            if( _clonedPlayerData == null)
            {
                _clonedPlayerData = Instantiate(playerStartData);
                _clonedPlayerData.Init(gameObject);
            }
            return _clonedPlayerData;
        }
    }

    private PlayerMovementController _playerMovementController;
    private PlayerResourceHandler playerResourceHandler;

    private void UseFirstItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (PlayerData.Consumable_1 == null) {
            return;
        }
        PlayerData.UseItem1();
    }

    private void UseSecondItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (PlayerData.Consumable_2 == null)
        {
            return;
        }
        PlayerData.UseItem2();
    }

    private void UseThirdItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (PlayerData.Consumable_3 == null)
        {
            return;
        }
        PlayerData.UseItem3();
    }

    private void Awake()
    {
        playerResourceHandler = GetComponent<PlayerResourceHandler>();
    }
    void Start()
    {
        PlayerInput playerInput = SInputManager.Instance.inputActions;
        playerInput.Keyboard._1.performed += UseFirstItem;
        playerInput.Keyboard._2.performed += UseSecondItem;
        playerInput.Keyboard._3.performed += UseThirdItem;
        UpdateStats();
        playerResourceHandler.SetData(PlayerData);
        if(_clonedPlayerData == null)
        {
            _clonedPlayerData = Instantiate(playerStartData);
            _clonedPlayerData.Init(gameObject);
        }
    }

    private void Update()
    {
        if (_clonedPlayerData.ForceUpdateStats)
        {
            UpdateStats();
            _clonedPlayerData.ForceUpdateStats = false;
        }
    }

    private void UpdateStats()
    {
        OnStatUpdate.Invoke(_clonedPlayerData);
    }
}
