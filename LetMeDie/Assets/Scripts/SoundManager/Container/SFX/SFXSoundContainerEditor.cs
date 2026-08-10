    using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SFXSoundContainer))]
public class SFXSoundContainerEditor : SoundContainerEditor
{
    public override void UpgradeList()
    {
        base.UpgradeList();
        SFXSoundContainer SFXSoundContainer = target as SFXSoundContainer;
        SFXSoundContainer.UpdateSoundList(typeof(SoundLibary.SFX));
    }
}
#endif
