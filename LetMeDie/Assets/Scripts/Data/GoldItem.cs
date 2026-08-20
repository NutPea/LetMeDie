using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items/Gold", order = 1)]
public class GoldItem : ItemData
{
    [SerializeField] private int minGoldAmount = 1;
    [SerializeField] private int maxGoldAmount = 10;
    public int GoldAmount => Random.Range(minGoldAmount,maxGoldAmount);

   
}
