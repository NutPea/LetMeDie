using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmbientSoundContainer", menuName = "Music/AmbientSoundContainer")]
public class AmbientSoundContainer : SoundContainer
{
    public Sound FindSound(SoundLibary.Ambient ambient)
    {
        return sounds[(int)ambient - 1];
    }
}
