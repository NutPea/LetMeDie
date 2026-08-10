using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(MusicSoundContainer))]
public class MusicSoundContainerEditor : SoundContainerEditor
{
    public override void UpgradeList()
    {
        base.UpgradeList();
        MusicSoundContainer musicSoundContainer = target as MusicSoundContainer;
        musicSoundContainer.UpdateSoundList(typeof(SoundLibary.Music));
    }
}
#endif
