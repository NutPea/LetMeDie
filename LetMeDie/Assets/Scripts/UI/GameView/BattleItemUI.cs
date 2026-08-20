using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;
    internal void SetUp(int key, BuffBattleLoot value)
    {
        text.text = "x" + key;
        icon.sprite = value.Icon;
    }
}
