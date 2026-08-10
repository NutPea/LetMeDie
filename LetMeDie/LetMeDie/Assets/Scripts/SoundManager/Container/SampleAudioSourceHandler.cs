using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SampleAudioSourceHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public bool hasBeenInit;

    private void Start()
    {
        if (hasBeenInit)
        {
            DestroyImmediate(gameObject);
        }
        hasBeenInit = true;
    }

    public void PlayAudioSource(AudioClip audioClip)
     {
        audioSource.clip = audioClip;
        audioSource.Play();
     }
    public void StopAudioSource()
    {
        audioSource.Stop();
    }
}
