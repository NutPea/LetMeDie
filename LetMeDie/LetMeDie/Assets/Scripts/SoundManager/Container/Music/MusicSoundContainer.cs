using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicSoundContainer", menuName = "Music/MusicSoundContainer")]
public class MusicSoundContainer : SoundContainer
{
    public Sound FindSound(SoundLibary.Music music)
    {
        return sounds[(int)music - 1];
    }
}
