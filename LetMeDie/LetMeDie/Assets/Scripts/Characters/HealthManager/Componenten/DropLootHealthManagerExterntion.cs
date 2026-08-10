using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class DropLootHealthManagerExterntion : MonoBehaviour
{
    [SerializeField] private List<ItemData> toDropItems;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private Transform spawnPosition;
    private HealthManager healthManager;
    [SerializeField] private float dropChance = 0.1f;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(GameObject diedObject)
    {
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        if (roll > dropChance) {
            return;
        }

        GameObject dropper = Instantiate(dropPrefab,spawnPosition.position,Quaternion.identity);
        ItemPickUpHandler itemPickUpHandler = dropper.GetComponent<ItemPickUpHandler>();


        itemPickUpHandler.SetItem(toDropItems[UnityEngine.Random.Range(0,toDropItems.Count-1)]);
    }

}
