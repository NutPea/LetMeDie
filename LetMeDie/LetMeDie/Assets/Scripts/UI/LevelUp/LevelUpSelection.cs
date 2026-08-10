using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpSelection : MonoBehaviour
{
    [SerializeField]private Button increaseValueButton;
    [SerializeField]private Button decreaseValueButton;

    private int startValue = 0;
    private int currentValue = 0;

    public delegate bool CheckIfSkillpointsAreAvailableDelegate();

    public CheckIfSkillpointsAreAvailableDelegate CheckIfSkillpointsAreAvailable;

    [SerializeField] private TextMeshProUGUI skillPointText;

    public UnityEvent<int> OnValueDecrease = new();
    public UnityEvent<int> OnValueIncrease = new();

    [SerializeField] private Color neutralColor = Color.white;
    [SerializeField] private Color increaseColor;

    private void Start()
    {
        increaseValueButton.onClick.AddListener(Increase);
        decreaseValueButton.onClick.AddListener(Decrease);
        increaseColor = SGameManager.Instance.IncreaseColor;

    }

    public void Init(CheckIfSkillpointsAreAvailableDelegate checkIfSkillpointsAreAvailable)
    {
        this.CheckIfSkillpointsAreAvailable = checkIfSkillpointsAreAvailable;
    }

    public void ChangeValuesOnNewUI(int value)
    {
        startValue = value;
        currentValue = value;
        UpdateText();
    }

    private void Decrease()
    {
        currentValue--;
        if(currentValue < startValue)
        {
            currentValue = startValue;
        }
        else
        {
            OnValueDecrease.Invoke(currentValue);
        }
        UpdateText();

    }

    private void Increase()
    {
        if (!CheckIfSkillpointsAreAvailable())
        {
            return;
        }
        currentValue++;
        OnValueIncrease.Invoke(currentValue);
        UpdateText();
    }

    private void UpdateText()
    {
        if (currentValue > startValue) {
            skillPointText.color = increaseColor;
        }
        else
        {
            skillPointText.color = neutralColor;
        }
        skillPointText.text = currentValue.ToString();
    }

}
