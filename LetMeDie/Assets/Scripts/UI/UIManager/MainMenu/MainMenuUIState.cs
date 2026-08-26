using Essentials;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIState : UIStateComponent
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    public override void OnInitUIState()
    {
        base.OnInitUIState();
        playButton.onClick.AddListener(() => SUIManager.Instance.ChangeToUIState("Play"));
        quitButton.onClick.AddListener(() => Application.Quit());   
    }

}
