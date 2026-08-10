using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAmbientToAudioSource : MonoBehaviour
{
   
    public SoundLibary.Ambient ambient;
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        Sound sound = SoundManager.instance.FindSound(ambient);
        sound.SetFirstClip(audioSource);
        audioSource.Play();
    }
}
