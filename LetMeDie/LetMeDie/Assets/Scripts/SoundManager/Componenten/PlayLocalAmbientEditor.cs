using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
[CustomEditor(typeof(PlayLocalAmbient))]
public class PlayLocalAmbientEditor : Editor
{
    bool ambientContainerUnassigned = false;
    bool showSoundsource;
    private PlayLocalAmbient playAmbient;
    private Editor audioSourceEditor;
    private AudioSource audioSource;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (ambientContainerUnassigned) EditorGUILayout.LabelField("Ambientcontainer of the SoundManager is Unassigned");
        if (EditorGUI.EndChangeCheck())
        {
            playAmbient = target as PlayLocalAmbient;
            if (playAmbient.ambient == SoundLibary.Ambient.None)
            {
                showSoundsource = false;
                return;
            }
            audioSource = playAmbient.GetComponent<AudioSource>();
            SoundManager soundManager = FindObjectOfType<SoundManager>();

            if (soundManager.ambientSoundContainer == null)
            {
                ambientContainerUnassigned = true;
                return;
            }
            else ambientContainerUnassigned = false;
            playAmbient.sound = soundManager.FindSound(playAmbient.ambient);
            playAmbient.sound.SetAudioSourceSettings(audioSource);
            playAmbient.sound.SetFirstClip(audioSource);
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
            playAmbient.sound.SetAudioSettings(audioSource);
        }
    }
}

#endif
