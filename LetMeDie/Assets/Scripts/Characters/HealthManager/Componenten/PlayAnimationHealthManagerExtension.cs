using System;
using UnityEngine;

public class PlayAnimationHealthManagerExtension : MonoBehaviour
{
    private HealthManager _healthManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _hitAnimationBool;
    [SerializeField] private string _deadAnimationBool;
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDamaged.AddListener(PlayAnimation);
        _healthManager.OnDeath.AddListener(Death);
    }

    private void Death(GameObject diedObject)
    {
        _animator.SetTrigger(_deadAnimationBool);
      //  Invoke(nameof(ResetDeathAnimation), 0.5f);
    }
    private void ResetDeathAnimation()
    {
        _animator.SetTrigger(_deadAnimationBool);
    }


    private void PlayAnimation(bool arg0, int arg1, Transform arg2)
    {
        if(_hitAnimationBool == "")
        {
            return;
        }
        _animator.SetBool(_hitAnimationBool, true);
        Invoke(nameof(ResetHitAnimation), 0.1f);
    }

    private void ResetHitAnimation()
    {
        _animator.SetBool(_hitAnimationBool, false);
    }

  
}
