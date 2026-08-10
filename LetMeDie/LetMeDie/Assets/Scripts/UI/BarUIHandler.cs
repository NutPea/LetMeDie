using TMPro;
using UnityEngine;

public class BarUIHandler : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private bool useText;

    private void Start()
    {
        healthText.gameObject.SetActive(useText);
    }

    public void SetValue(int currentValue,int maxValue)
    {
        if(currentValue == 0)
        {
            pivot.transform.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            float perncentageValue = (float)currentValue / (float)maxValue;
            pivot.transform.localScale = new Vector3(perncentageValue, 1, 1);
        }


        if (useText)
        {
            healthText.text = currentValue +" / " + maxValue;
        }
    }
}
