using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterSFXSoundContainer))]
public class CharacterSFXSoundContainerEditor : SoundContainerEditor
{
    public override void UpgradeList()
    {
        base.UpgradeList();
        CharacterSFXSoundContainer CharacterSFXSoundContainer = target as CharacterSFXSoundContainer;
        CharacterSFXSoundContainer.UpdateSoundList(typeof(SoundLibary.CharacterSFX));
    }
}
#endif
