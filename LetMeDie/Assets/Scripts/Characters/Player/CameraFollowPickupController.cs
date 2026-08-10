using System;
using UnityEngine;

public class CameraFollowPickupController : MonoBehaviour
{
    [SerializeField] private GameObject playerBody;
    private PlayerPickUpHandler playerPickUpHandler;


    [SerializeField] private Transform _camera;
    [SerializeField] private float offsetSpeed = 10f;
    [SerializeField] private float extraDistance = 2f;

    private PickupHandler currentPickUpHandler;

    private void Start()
    {
        playerPickUpHandler = playerBody.GetComponent<PlayerPickUpHandler>();
        playerPickUpHandler.OnPickUp.AddListener(PickUpObject);
        playerPickUpHandler.OnDrop.AddListener(Drop);
    }

    private void Drop()
    {
        currentPickUpHandler.Drop();
    }

    private void PickUpObject(PickupHandler pickupHandler)
    {
        currentPickUpHandler = pickupHandler;
        currentPickUpHandler.PickUp(transform);

    }

}
