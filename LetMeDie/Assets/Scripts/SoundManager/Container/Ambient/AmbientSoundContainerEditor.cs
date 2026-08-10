using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AmbientSoundContainer))]
public class AmbientSoundContainerEditor : SoundContainerEditor
{
    public override void UpgradeList()
    {
        base.UpgradeList();
        AmbientSoundContainer ambientSoundContainer = target as AmbientSoundContainer;
        ambientSoundContainer.UpdateSoundList(typeof(SoundLibary.Ambient));
    }
}
#endif
