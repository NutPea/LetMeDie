
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Essentials {
    public class SUIManager : MonoBehaviour {


        public enum DeviceHandlingMode
        {
            None = -1,
            HandleCursor = 0
        }

        [Header("Device")]
        private InputDevice currentInputDevice = null;
        public DeviceHandlingMode DeviceHandling;

        public static SUIManager Instance;
        [HideInInspector] public UIState PreviouseUIState;
        public UIState CurrenUIState;

        public List<UIState> UIStates;

        [SerializeField] private bool startWithUiState;
        [SerializeField] private string startUIState = "Game";
        private const string EMPTY_STATE = "None";

        [SerializeField] private bool debug;

        /// <summary>
        /// First UI State is the old one the second the new one
        /// </summary>
        [HideInInspector] public UnityEvent<UIState, UIState> OnUIStateChanged = new();
        [HideInInspector] public UnityEvent<GameObject, GameObject> OnSelectionChanged = new();
        public UnityEvent OnInitFinished = new();
        private GameObject currentSelectedGameobject;

        public const string GAME_UI_STATENAME = "Game";

        public const string DEATH_UI_STATENAME = "Death";

        public const string STATS_UI_STATENAME = "Stats";

        public const string EQUIPMENT_UI_STATENAME = "Equipment";

        public const string MENU_UI_STATENAME = "Menu";

        public const string SELECTION_UI_STATENAME = "Selection";

        public const string CHECKPOINT_UI_STATENAME = "Checkpoint";

        public const string LEVEL_UP_UI_STATENAME = "LevelUp";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitAwakeUIStates();
            }
            else
            {
                Destroy(gameObject);
            }
            UIState empty = new UIState();
            empty.UIStateName = EMPTY_STATE;
            CurrenUIState = empty;

        }

        private void Start()
        {
            if (Instance == this)
            {
                InitUIStates();

                if (startWithUiState)
                {
                    ChangeToUIState(startUIState);
                }
                else {
                    UIState empty = new UIState();
                    empty.UIStateName = EMPTY_STATE;
                    CurrenUIState = empty;
                }
                OnInitFinished.Invoke();
            }
        }


  



        private void Update()
        {
            if(EventSystem.current.currentSelectedGameObject != currentSelectedGameobject)
            {
                GameObject lastSelectedObject = currentSelectedGameobject;
                currentSelectedGameobject = EventSystem.current.currentSelectedGameObject;
                OnSelectionChanged.Invoke(lastSelectedObject, EventSystem.current.currentSelectedGameObject);
            }
        }

        private void InitAwakeUIStates()
        {

            foreach (UIState uIState in UIStates)
            {
                uIState.UIStateObject.SetActive(true);
                uIState.OnAwakeInit();
                uIState.UIStateObject.SetActive(false);
            }
        }

        private void InitUIStates()
        {

            foreach (UIState uIState in UIStates)
            {
                uIState.UIStateObject.SetActive(true);
                uIState.OnInit();
                uIState.UIStateObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            foreach (UIState uIState in UIStates)
            {
                uIState.OnCleanup();
            }

        }


        public void ChangeToUIState(string UIStateName)
        {
            foreach (UIState state in UIStates)
            {
                if (UIStateName == state.UIStateName && UIStateName != CurrenUIState.UIStateName)
                {
                    PreviouseUIState = CurrenUIState;
                    if (PreviouseUIState.UIStateName != EMPTY_STATE)
                    {
                        PreviouseUIState.OnExit();
                        PreviouseUIState.UIStateObject.SetActive(false);
                    }
                    CurrenUIState = state;
                    CurrenUIState.OnBeforeEnter();
                    CurrenUIState.UIStateObject.SetActive(true);
                    CurrenUIState.OnEnter();
                    OnUIStateChanged.Invoke(PreviouseUIState, CurrenUIState);

                    if (debug) {
                        Debug.Log($"Changed from {PreviouseUIState.UIStateName} to {CurrenUIState.UIStateName}");
                    }
                }
            }
        }

        private IEnumerator WaitForCamera(float time, UIState prev , UIState curr)
        {
            yield return new WaitForSecondsRealtime(time);
            if (curr.UIStateName != "None") {
                curr.UIStateObject.SetActive(true);
                curr.OnEnter();
                OnUIStateChanged.Invoke(prev, curr);
            }
        }

    }


    [System.Serializable]
    public class UIState
    {
        public string UIStateName;
        public GameObject UIStateObject;
        private UIStateComponent _uiStateComponent;
        private UIStateComponent UIStateComponent {
            get {
                if (_uiStateComponent == null) {
                    _uiStateComponent = UIStateObject.GetComponent<UIStateComponent>();
                }

                return _uiStateComponent;
            }
        }
        public bool WaitForCamera {

            get
            {
                if(_uiStateComponent == null)
                {
                    return false;
                }
                return _uiStateComponent.WaitUntilCameraMoved;
            }
        }

        public void OnAwakeInit()
        {
            UIStateComponent.OnAwakeInitUIState();
        }


        public void OnInit()
        {
            UIStateComponent.OnInitUIState();
        }


        public void OnExit()
        {
            UIStateComponent?.OnExitUIState();
        }

        public void OnBeforeEnter()
        {
            UIStateComponent?.OnBeforeEnterUIState();
        }

        public void OnEnter()
        {
            UIStateComponent?.OnEnterUIState();
        }

        public void OnCleanup()
        {
            UIStateComponent?.OnCleanupUIState();
        }
    }
}

