
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Essentials
{
    public class SLoadManager : MonoBehaviour
    {
        public static SLoadManager Instance;

        private static int _toLoadLevel;
        public static int ToLoadLevel => _toLoadLevel;
        private static string _toLoadLevelSceneName;
        public static string ToLoadLevelSceneName => _toLoadLevelSceneName;
        public static int LastLoadedLevelIndex;
        public UnityEvent OnCleanup = new();
        public UnityEvent OnEnterNewScene = new();


        [SerializeField] private float transitionTime = 0.5f;
        [SerializeField] private Image transitionImage;
        [SerializeField] private LeanTweenType tweenType;



        public enum LevelName
        {
            Loading_Screen = -2,
            None = -1,
            MainMenu = 0,
            DungeonScene_1 = 1,
            DungeonScene_2 = 2,
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            if (transitionImage == null)
            {
                LoadScene(GetSceneIndexByName(sceneName));
            }
            else
            {
                transitionImage.gameObject.SetActive(true);
                LeanTween.value(gameObject, 0, 1, transitionTime).setOnUpdate((float val) =>
                {
                    Color c = transitionImage.color;
                    c.a = val;
                    transitionImage.color = c;
                }).setEase(tweenType).setOnComplete(() =>
                {
                    LoadScene(GetSceneIndexByName(sceneName));
                }).setIgnoreTimeScale(true);
            }
        }

        public void ReloadScene() {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void HideTransitionImage()
        {
            if (transitionImage)
            {
                transitionImage.gameObject.SetActive(false);
            }
        }

        private int maxIterations = 3;
        private int currentIterations = 0;
        public int GetSceneIndexByName(string sceneName)
        {
            int sceneNumber = SceneManager.sceneCountInBuildSettings;
            currentIterations++;
            for (int possibleSceneIndex = 0; possibleSceneIndex < sceneNumber; possibleSceneIndex++)
            {
                string scenePath = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(possibleSceneIndex));
                int slash = scenePath.LastIndexOf('/');
                string name = scenePath.Substring(slash + 1);

                if (name == sceneName)
                {
                    currentIterations = 0;
                    return possibleSceneIndex;
                }
            }

            if(currentIterations >= maxIterations) {
                return -1;
            }

            Debug.LogError($"You cant load the Scene Name : {sceneName} + You will load into the MainMenu");
            return GetSceneIndexByName("MainMenu");
        }

        public string GetSceneNameByIndex(int sceneIndex)
        {
            string scenePath = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
            int slash = scenePath.LastIndexOf('/');
            string name = scenePath.Substring(slash + 1);
            return name;
        }


        public string GetActiveSceneName()
        {
           return GetSceneNameByIndex(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadScene(LevelName levelName)
        {
            switch (levelName)
            {
                case LevelName.MainMenu: LoadScene("MainMenu"); break;
                case LevelName.DungeonScene_1: LoadScene("DungeonScene_1"); break;
                case LevelName.DungeonScene_2: LoadScene("DungeonScene_2"); break;
            }


        }


        public void LoadScene(int sceneIndex)
        {
            if (sceneIndex == -1)
            {
                Debug.LogError($"You cant load the index : {sceneIndex}");
            }
            OnCleanup.Invoke();
            _toLoadLevel = sceneIndex;
            _toLoadLevelSceneName = GetSceneNameByIndex(sceneIndex);
            LastLoadedLevelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(GetSceneIndexByName("LoadingScreen"));
            Time.timeScale = 1;
        }


    }
}
