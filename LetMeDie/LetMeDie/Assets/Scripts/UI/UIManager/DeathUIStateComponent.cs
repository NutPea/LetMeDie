using Essentials;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUIStateComponent : UIStateComponent
{
    [SerializeField] private Button restart;

    public override void OnInitUIState()
    {
        PlayerSingelton.instance.gameObject.GetComponent<HealthManager>().OnDeath.AddListener((GameObject diedObject) => SUIManager.Instance.ChangeToUIState("Death"));
        restart.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public override void OnEnterUIState()
    {
        base.OnEnterUIState();
        Time.timeScale = 0.0f;
        SGameManager.Instance.SetCursorVisibility(true, CursorLockMode.None);
        SGameManager.Instance.PlayerIsDead = true;
    }

}
