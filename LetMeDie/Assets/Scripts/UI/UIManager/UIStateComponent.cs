using Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateComponent : MonoBehaviour
{

    public enum CursoreMode
    {
        None = -1,
        HandleCursor = 0,
        AlwaysHideCursor = 1
    }

    [SerializeField] private CursoreMode deviceMode;

    public virtual void OnAwakeInitUIState()
    {

    }

    public virtual void OnInitUIState()
    {

    }
    public virtual void OnBeforeEnterUIState()
    {
    }

    public virtual void OnEnterUIState()
    {
        IsUIStateActive = true;
    }
    public virtual void OnExitUIState()
    {
        IsUIStateActive = false;
    }
    public virtual void OnCleanupUIState()
    {

    }


    protected bool IsUIStateActive = false;

    public virtual bool WaitUntilCameraMoved
    {
        get => false;
    }
}
