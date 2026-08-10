using System;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class MovingReactionHealthManagerExtention : MonoBehaviour
{
    HealthManager _healthManager;

    [SerializeField] private float movingAmount;
    [SerializeField] private float movingTime;
    [SerializeField] private LeanTweenType movingType;

    private Vector3 moveDirection;
    private Vector3 startPosition;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDamaged.AddListener(OnDamageTaken);
    }

    private void OnDamageTaken(bool arg0, int arg1,float knockBack, Transform hitPosition)
    {
        Vector3 hitDirection = transform.position - hitPosition.position;
        hitDirection = hitDirection.normalized;
        startPosition = transform.position;

        moveDirection = transform.position + hitDirection * movingAmount;
        LeanTween.move(gameObject, moveDirection, movingTime).setEase(movingType).setOnComplete(MoveBack);
    }

    private void MoveBack()
    {
        LeanTween.move(gameObject, startPosition, movingTime).setEase(movingType);
    }


}
