using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SFXSSoundContainer", menuName = "Music/SFXSSoundContainer")]
public class SFXSoundContainer : SoundContainer
{
    public Sound FindSound(SoundLibary.SFX sfx)
    {
        return sounds[(int)sfx - 1];
    }
}
