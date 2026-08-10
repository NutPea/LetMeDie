using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
[CustomEditor(typeof(PlayLocalMusic))]
public class PlayLocalMusicEditor : Editor
{

    bool musicContainerUnassigned = false;
    bool showSoundsource;
    private PlayLocalMusic playMusic;
    private Editor audioSourceEditor;
    private AudioSource audioSource;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (musicContainerUnassigned) EditorGUILayout.LabelField("MusicContainer of the SoundManager is Unassigned");
        if (EditorGUI.EndChangeCheck())
        {
            playMusic = target as PlayLocalMusic;
            if (playMusic.music == SoundLibary.Music.None)
            {
                showSoundsource = false;
                return;
            }
            audioSource = playMusic.GetComponent<AudioSource>();
            SoundManager soundManager = FindObjectOfType<SoundManager>();
           
            if (soundManager.musicSoundContainer == null)
            {
                musicContainerUnassigned = true;
                return;
            }
            else musicContainerUnassigned = false;
            playMusic.sound = soundManager.FindSound(playMusic.music);
            playMusic.sound.SetAudioSourceSettings(audioSource);
            playMusic.sound.SetFirstClip(audioSource);
            audioSourceEditor = Editor.CreateEditor(audioSource);
            showSoundsource = true;
        }
        if (showSoundsource == false) return;

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");

        EditorGUI.BeginChangeCheck();
        audioSourceEditor.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            playMusic.sound.SetAudioSettings(audioSource);
        }


    }

}

#endif