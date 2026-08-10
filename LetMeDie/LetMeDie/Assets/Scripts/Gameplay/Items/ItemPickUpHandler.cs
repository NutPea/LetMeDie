using System;
using UnityEngine;

public class ItemPickUpHandler : MonoBehaviour
{

    [SerializeField] private ItemData _itemData;
    private bool _isPickup;

    internal void SetItem(ItemData toDropItem)
    {
        _itemData = toDropItem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPickup)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(_itemData.ItemName);
            SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData.AddItem(_itemData);
            _isPickup = true;
            Destroy(gameObject);
        }
    }

}
