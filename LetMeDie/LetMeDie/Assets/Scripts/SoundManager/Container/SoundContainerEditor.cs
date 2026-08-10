using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
public class SoundContainerEditor : Editor
{
    SampleAudioSourceHandler sampleAudioSourceHandler;
    bool extraContainerOptions;
    AudioMixerGroup audioMixerGroup;
    private float volume = 1;
    private float pitch = 1;
    private void OnEnable()
    {
        UpgradeList();
    }

    public override void OnInspectorGUI()
    {
        if (extraContainerOptions)
        {
            audioMixerGroup = EditorGUILayout.ObjectField("All AudioMixerGroups", audioMixerGroup, typeof(AudioMixerGroup), true) as AudioMixerGroup;
            volume = EditorGUILayout.FloatField("All volumes", volume);
            pitch = EditorGUILayout.FloatField("All pitches", pitch);

            if (GUILayout.Button("Upgrade all audioSourceSettings in container"))
            {
                SoundContainer soundContainer = target as SoundContainer;
                foreach (Sound sound in soundContainer.sounds)
                {
                    sound.audioSetting.output = audioMixerGroup;
                    sound.audioSetting.volume = volume;
                    if (pitch != 0) sound.audioSetting.pitch = pitch;
                    else sound.audioSetting.pitch = 1;
                }
                extraContainerOptions = false;
            }
            if (GUILayout.Button("Hide extra container options"))
            {
                extraContainerOptions = false;
            }
        }
        else
        {
            if (GUILayout.Button("Show extra Container Options"))
            {
                extraContainerOptions = true;
            }
        }

        

        if (GUILayout.Button("Manually update List"))
        {
            UpgradeList();
        }

        if (GUILayout.Button("SetDefaultAudioSource"))
        {
            SoundContainer soundContainer = target as SoundContainer;
            foreach (Sound sound in soundContainer.sounds)
            {
                if(sound.audioSetting.output == null)
                {
                    sound.audioSetting.output = soundContainer.defaultMixerGroup;
                }
            }
            EditorUtility.SetDirty(soundContainer);

        }

        base.OnInspectorGUI();
        CheckForPlayButtons();
        CheckForStopButtons();
    }

    public virtual void UpgradeList()
    {

    }

    public void CheckForPlayButtons()
    {
        SoundContainer soundContainer = target as SoundContainer;
        foreach(Sound sound in soundContainer.sounds)
        {
            if (sound.playFirstClip)
            {
                if(sampleAudioSourceHandler == null)
                {
                    GameObject audioSourceGameobject = new GameObject("AudioSourceSample");
                    sampleAudioSourceHandler = audioSourceGameobject.AddComponent<SampleAudioSourceHandler>();
                    AudioSource audioSource = audioSourceGameobject.AddComponent<AudioSource>();
                    sampleAudioSourceHandler.audioSource = audioSource;
                }
                sound.SetAudioSourceSettings(sampleAudioSourceHandler.audioSource);
                sampleAudioSourceHandler.PlayAudioSource(sound.soundClips[0].audioClip);
                sound.playFirstClip = false;
                break;
            }
        }
    }


    public void CheckForStopButtons()
    {
        SoundContainer soundContainer = target as SoundContainer;
        foreach (Sound sound in soundContainer.sounds)
        {
            if (sound.stopFirstClip)
            {
                if (sampleAudioSourceHandler == null)
                {
                    GameObject audioSourceGameobject = new GameObject("AudioSourceSample");
                    sampleAudioSourceHandler = audioSourceGameobject.AddComponent<SampleAudioSourceHandler>();
                    AudioSource audioSource = audioSourceGameobject.AddComponent<AudioSource>();
                    sampleAudioSourceHandler.audioSource = audioSource;
                }
                sampleAudioSourceHandler.StopAudioSource();
                sound.stopFirstClip = false;
                break;
            }
        }
    }

}
#endif
