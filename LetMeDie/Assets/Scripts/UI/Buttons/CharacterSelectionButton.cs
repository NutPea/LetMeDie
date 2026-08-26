using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{
    private PlayerData playerData;
    [SerializeField]private Image characterImage;
    [HideInInspector] public UnityEvent<PlayerData> OnCharacterSelect = new();
    [SerializeField] private Button selectionButton;

    private void Start()
    {
        selectionButton.onClick.AddListener(() => OnCharacterSelect.Invoke(playerData));
    }

    public void Setup(PlayerData playerData)
    {
        this.playerData = playerData;
        characterImage.sprite = playerData.Icon;
    }


}
