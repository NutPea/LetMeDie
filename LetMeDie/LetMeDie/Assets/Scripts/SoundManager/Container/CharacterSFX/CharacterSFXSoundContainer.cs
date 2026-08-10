using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXSSoundContainer", menuName = "Music/CharacterSFXSSoundContainer")]
public class CharacterSFXSoundContainer : SoundContainer
{
    public Sound FindSound(SoundLibary.CharacterSFX characterSFX)
    {
        return sounds[(int)characterSFX - 1];
    }
}
