using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip 
{
    public AudioClip audioClip;
    public int value = 100;

    public SoundClip()
    {
        value = 100;
    }

    public float GetPercentageValue()
    {
        return (float)value/100.0f;
    }



}
