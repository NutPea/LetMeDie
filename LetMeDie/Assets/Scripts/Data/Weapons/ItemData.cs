using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] private string _guid;
    public string GUID => _guid;

    [SerializeField] private string itemName = "NO NAME";
    public string ItemName => itemName;

    [SerializeField] private string description = "NO DESCRIPTION";
    public string Description => description;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private Color _tint = Color.white;
    public Color Tint => _tint;
}
