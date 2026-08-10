using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthManager))]
public class DestroyOnDeathHealthManagerExtension : MonoBehaviour
{
    HealthManager _healthManager;
    public float timer = 0.05f;
    [SerializeField] private bool setsInactiveInsteadOfDestroy = true;

    public UnityEvent onDeathStart = new UnityEvent();
    public UnityEvent onDeathRemove = new UnityEvent();
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDeath.AddListener(Destroing);
    }

    private void Destroing(GameObject diedObject)
    {
        StartCoroutine(DestroyingCouroutine());
    }

    IEnumerator DestroyingCouroutine()
    {
        onDeathStart.Invoke();
        yield return new WaitForSeconds(timer);
        onDeathRemove.Invoke();
        if (setsInactiveInsteadOfDestroy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
