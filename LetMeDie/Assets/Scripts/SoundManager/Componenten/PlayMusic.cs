using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public SoundLibary.Music music;
    void Start()
    {
        SoundManager.instance.PlayMainMusic(music);
    }


}
