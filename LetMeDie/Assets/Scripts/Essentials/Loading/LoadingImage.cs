using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LoadingImage : MonoBehaviour
    {
        [SerializeField] private float verticalMovementAmount = 1;
        [SerializeField] private LeanTweenType fallLeanTyp;
        [SerializeField] private float fallTime;
        [SerializeField] private LeanTweenType upLeanTyp;
        [SerializeField] private float upTime;


        private Vector3 startPosition;
        private void Start()
        {
            startPosition = transform.localPosition;
            GoDown();


        }

        private void GoDown()
        {
            LeanTween.moveLocal(gameObject, startPosition + new Vector3(0,-verticalMovementAmount,0), fallTime).setEase(fallLeanTyp).setOnComplete(GoUp);

        }

        private void GoUp()
        {
            LeanTween.moveLocal(gameObject, startPosition + new Vector3(0, verticalMovementAmount, 0), upTime).setEase(fallLeanTyp).setOnComplete(GoDown);
        }
    }

}
