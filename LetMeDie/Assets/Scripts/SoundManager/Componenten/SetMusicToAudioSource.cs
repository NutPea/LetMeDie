using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicToAudioSource : MonoBehaviour
{
    public SoundLibary.Music music;
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        Sound sound = SoundManager.instance.FindSound(music);
        sound.SetFirstClip(audioSource);
        audioSource.Play();
    }

}
