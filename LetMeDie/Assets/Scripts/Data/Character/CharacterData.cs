using UnityEngine;

public class CharacterData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField] private string characterName = "No Name";
    public string Name => characterName;

}
