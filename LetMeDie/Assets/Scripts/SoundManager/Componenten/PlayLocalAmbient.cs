using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayLocalAmbient : MonoBehaviour
{
    public SoundLibary.Ambient ambient;
    public Sound sound;

    public void Start()
    {
        SoundManager.instance.PlayLibarySound(ambient);
    }
}
