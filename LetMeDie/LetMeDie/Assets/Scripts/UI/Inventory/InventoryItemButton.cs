using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    private ItemData _data;
    public ItemData Data => _data;
    public Button button;
    public void SetItem(ItemData data)
    {
        if (data == null) {
            _data = null;
            image.sprite = null;
            image.gameObject.SetActive(false); 
            text.text = "---";
        }
        else
        {
            _data = data;
            image.sprite = data.Sprite;
            text.text = data.ItemName;
            image.gameObject.SetActive(true);
        }
    }


}
