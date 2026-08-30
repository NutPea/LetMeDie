using Essentials;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Essentials {
    public class LoadingScreenHandler : MonoBehaviour
    {
        [SerializeField] private Image transitionPanel;
        [SerializeField] private float transitionTime = 0.5f;



        void Start()
        {
            TransitionOut();
            SLoadManager.Instance.HideTransitionImage();
            // StartLoading();
        }


        public void TransitionOut()
        {
            Time.timeScale = 0.0f;
            transitionPanel.gameObject.SetActive(true);
            LeanTween.value(gameObject, 1, 0, transitionTime).setOnUpdate((float val) =>
            {
                Color c = transitionPanel.color;
                c.a = val;
                transitionPanel.color = c;
            }).setOnComplete(StartLoading).setIgnoreTimeScale(true);
        }

        public void StartLoading()
        {
            StartCoroutine(StartLoadingCouroutine());
        }

        IEnumerator StartLoadingCouroutine()
        {
            if (SLoadManager.ToLoadLevel == -1)
            {
                Debug.LogError("No Scene Index has been set");
            }
        
            AsyncOperation operation = SceneManager.LoadSceneAsync(SLoadManager.ToLoadLevel, LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                SetLoadingProgress(Mathf.Clamp01(operation.progress / 0.9f));
                yield return null;
            }
            RemoveScene();
           // TransitionIn();
        }

        public void TransitionIn()
        {
            transitionPanel.gameObject.SetActive(true);
            LeanTween.value(gameObject, 1, 0, transitionTime).setOnUpdate((float val) =>
            {
                Color c = transitionPanel.color;
                c.a = val;
                transitionPanel.color = c;
            }).setOnComplete(RemoveScene).setIgnoreTimeScale(true);
        }

        public void RemoveScene()
        {
            SLoadManager.Instance.OnEnterNewScene.Invoke();
            SceneManager.UnloadScene(SLoadManager.Instance.GetSceneIndexByName("LoadingScreen"));
            Time.timeScale = 1.0f;
        }

        private void SetLoadingProgress(float progress)
        {
           // LoadingProgressionPivot.localScale = new Vector3(progress, 1, 1);
        }
    }
}
