using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items/Gold", order = 1)]
public class GoldItem : ItemData
{
    [SerializeField] private int goldAmount;
    public int GoldAmount => goldAmount;

    public void AddGold(int gold)
    {
        goldAmount += gold;
    }
}
