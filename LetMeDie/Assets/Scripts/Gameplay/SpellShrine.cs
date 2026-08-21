using Essentials;
using UnityEngine;

public class SpellShrine : MonoBehaviour
{
    [SerializeField] private float timeUntilFullyCharged;
    private float currentTimeUntilFullyCharged = 0;

    private bool isCharging = false;
    private bool canCharge = true;
    [SerializeField] private GameObject rangeIndikator;
    [SerializeField] private GameObject fillIndikator;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {

        if (!canCharge)
        {
            return;
        }
        if (isCharging)
        {
            if(currentTimeUntilFullyCharged < 0)
            {
                currentTimeUntilFullyCharged = timeUntilFullyCharged;
                canCharge = false;
                SUIManager.Instance.ChangeToUIState("SpellLevelUp");
                rangeIndikator.gameObject.SetActive(false);
                fillIndikator.gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                currentTimeUntilFullyCharged -= Time.deltaTime;
                float percentage = 1- currentTimeUntilFullyCharged / timeUntilFullyCharged;
                fillIndikator.transform.localScale = new Vector3(percentage, percentage, percentage);
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (!canCharge)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            currentTimeUntilFullyCharged = timeUntilFullyCharged;
            rangeIndikator.gameObject.SetActive(true);
            fillIndikator.gameObject.SetActive(true);
            fillIndikator.transform.localScale = new Vector3(0, 0, 0);
            isCharging = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!canCharge)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Hide();
        }
    }

    private void Hide()
    {
        rangeIndikator.gameObject.SetActive(false);
        fillIndikator.gameObject.SetActive(false);
        isCharging = false;
    }
}
