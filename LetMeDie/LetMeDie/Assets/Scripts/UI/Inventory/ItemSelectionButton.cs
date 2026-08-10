using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSelectionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private ItemData currentData;

    public UnityEvent<ItemData> OnSetItem = new UnityEvent<ItemData>();

    public void Init()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSetItem.Invoke(currentData));
    }

    public void SetItem(ItemData data)
    {
        currentData = data;
        text.text = data.ItemName;
    }

}
