using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingelton : MonoBehaviour
{
    public static PlayerSingelton instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public GameObject GetPlayerObject()
    {
        return this.gameObject;
    }
}
