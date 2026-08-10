using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelUpDescriptionSelection : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [HideInInspector] public UnityEvent<string> OnDescriptionChange = new();
    [SerializeField] private string m_Description;
    public string Description { get { return m_Description; } }

    public void OnSelect(BaseEventData eventData)
    {
        OnDescriptionChange.Invoke(m_Description);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnDescriptionChange.Invoke(m_Description);
    }
}
