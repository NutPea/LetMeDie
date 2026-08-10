using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public SoundLibary.SFX soundEffect;
    public void _PlaySoundEffect()
    {
        SoundManager.instance.PlayLibarySound(soundEffect);
    }

    public void _PlaySoundEffectRepeat()
    {
        SoundManager.instance.PlayContinuousLibarySound(soundEffect);
    }

    public void _PlaySoundEffectRandome()
    {
        SoundManager.instance.PlayRandomLibarySound(soundEffect);
    }
}
