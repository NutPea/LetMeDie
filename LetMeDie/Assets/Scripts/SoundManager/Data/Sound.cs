using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public string name;

    [Space(10)]
    public List<SoundClip> soundClips;
    [Header("Plays first soundclip in list")]
    public bool playFirstClip = false;
    [Header("Stops first soundclip in list")]
    public bool stopFirstClip = false;
    [Space(10)]
    [Header("Continious")]
    [HideInInspector] public bool inCouroutine;
    [HideInInspector] public bool canRepeatItself = false;
    public float repeatTimer = 0.1f;
    [HideInInspector]public List<AudioSource> usedAudioSources = new List<AudioSource>();


    [Space(10)]
    public AudioSourceSettings audioSetting;

    /// <summary>
    /// Gets index of used audiosource index
    /// </summary>
    private int GetCurrentUsedLayeredAudioSourceIndex(float percentageValue)
    {
        for(int i = 0; i< soundClips.Count; i++)
        {
            float soundClipPercentageValue = soundClips[i].GetPercentageValue();
            if (soundClipPercentageValue > percentageValue) return i;
        }
        return soundClips.Count-1;
    }

    /// <summary>
    /// Sets volume of currently layered audiosource depending on percentage
    /// </summary>
    public void SetLayeredAudioSourceVolume(float percentage)
    {
        int layeredIndex = GetCurrentUsedLayeredAudioSourceIndex(percentage);
        AudioSource layeredAudioSource = usedAudioSources[layeredIndex];

        float volumePercentage = 0;
        if (layeredIndex == 0) volumePercentage = percentage / soundClips[layeredIndex].GetPercentageValue();
        else
        {
            volumePercentage = (percentage - soundClips[layeredIndex-1].GetPercentageValue()) /
                (soundClips[layeredIndex].GetPercentageValue() - soundClips[layeredIndex-1].GetPercentageValue());
        }

        float newVolume = audioSetting.volume * volumePercentage;
        if (newVolume > audioSetting.volume) newVolume = audioSetting.volume;
        layeredAudioSource.volume = newVolume;
    }

    /// <summary>
    /// Sets up the audiosource with sound audiosettings
    /// </summary>
    public void SetAudioSourceSettings(AudioSource source)
    {

        source.outputAudioMixerGroup = audioSetting.output;

        source.playOnAwake = audioSetting.playOnAwake;
        source.bypassEffects = audioSetting.bypassEffects;
        source.bypassListenerEffects = audioSetting.bypassListenerEffects;
        source.bypassReverbZones = audioSetting.bypassReverbZones;
        source.loop = audioSetting.loop;

        source.priority = audioSetting.priority;
        source.volume = audioSetting.volume;
        source.pitch = audioSetting.pitch;
        source.panStereo = audioSetting.stereoPan;
        source.spatialBlend = audioSetting.spatialBlend;
        source.reverbZoneMix = audioSetting.reverbZoneMix;

        source.dopplerLevel = audioSetting.dopplerLevel;
        source.spread = audioSetting.spread;
        source.rolloffMode = audioSetting.volumeRolloff;
        source.minDistance = audioSetting.minDistance;
        source.maxDistance = audioSetting.maxDistance;

        if (audioSetting.volumeRolloff == AudioRolloffMode.Custom)
        {
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSetting.RolloffCustomCurve);
            source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, audioSetting.ReverbZoneMixCustomCurve);

            source.SetCustomCurve(AudioSourceCurveType.Spread, audioSetting.SpreadCustomCurve);
            source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, audioSetting.SpartialBlendCurve);
        }
    }

    /// <summary>
    /// Sets sound audiosettings with audiosource 
    /// </summary>
    public void SetAudioSettings(AudioSource source)
    {
        audioSetting.output = source.outputAudioMixerGroup;
        audioSetting.playOnAwake = source.playOnAwake;
        audioSetting.bypassEffects = source.bypassEffects;
        audioSetting.bypassListenerEffects = source.bypassListenerEffects;
        audioSetting.bypassReverbZones = source.bypassReverbZones;
        audioSetting.loop = source.loop;

        audioSetting.priority = source.priority;
        audioSetting.volume = source.volume;
        audioSetting.pitch = source.pitch;
        audioSetting.stereoPan = source.panStereo;
        audioSetting.spatialBlend = source.spatialBlend;
        audioSetting.reverbZoneMix = source.reverbZoneMix;

        audioSetting.dopplerLevel = source.dopplerLevel;
        audioSetting.spread = (int)source.spread;
        audioSetting.volumeRolloff = source.rolloffMode;
        audioSetting.minDistance = source.minDistance;
        audioSetting.maxDistance = source.maxDistance;

        audioSetting.RolloffCustomCurve = source.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
        audioSetting.ReverbZoneMixCustomCurve = source.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix);

        audioSetting.SpreadCustomCurve = source.GetCustomCurve(AudioSourceCurveType.Spread);
        audioSetting.SpartialBlendCurve = source.GetCustomCurve(AudioSourceCurveType.SpatialBlend);
    }

    /// <summary>
    /// Sets the first clip in list
    /// </summary>
    public void SetFirstClip(AudioSource source)
    {
        if(soundClips.Count > 0)
        {
            source.clip = soundClips[0].audioClip;
        }
        else
        {
            Debug.Log("No Audio ClipAssigned to sound : !" + name);
        }
    }

    public void SetFirstAudioSource(AudioSource audioSource)
    {
        if (usedAudioSources.Count == 0) usedAudioSources.Add(audioSource);
        else usedAudioSources[0] = audioSource;
    }

    public SoundClip GetRandomSoundClip()
    {
        if (soundClips.Count == 1) return soundClips[0];

        int randomeValue = Random.Range(0, 100);
        int currentCheckValue = soundClips[0].value;
        for(int i = 0;i<soundClips.Count; i++)
        {
            if (randomeValue < currentCheckValue) return soundClips[i];
            else currentCheckValue += soundClips[i].value;
        }
        return soundClips[soundClips.Count - 1];
    }

    public void SetRandomClipToAudioSource(AudioSource audioSource)
    {
        SoundClip randomeSoundClip = GetRandomSoundClip();
        audioSource.clip = randomeSoundClip.audioClip;
    }

    public void RemoveEmptysInUsedAudioSource()
    {
        usedAudioSources = usedAudioSources.Where(item => item != null).ToList();
    }

}

[System.Serializable]
public class AudioSourceSettings
{

    public AudioMixerGroup output;

    public bool bypassEffects;
    public bool bypassListenerEffects;
    public bool bypassReverbZones;
    public bool playOnAwake;
    public bool loop;

    [Space(10)]
    [Header("High <-> Low")]
    [Range(0, 256)]
    public int priority = 128;

    [Space(10)]
    [Range(0f, 1f)]
    public float volume = 1;

    [Space(10)]
    [Range(-3f, 3f)]
    public float pitch = 1;

    [Space(10)]
    [Header("Left <-> Right")]
    [Range(-1f, 1f)]
    public float stereoPan = 0;

    [Space(10)]
    [Header("2D <-> 3D")]
    [Range(0f, 1f)]
    public float spatialBlend = 0;

    [Space(10)]
    [Range(0f, 1.1f)]
    public float reverbZoneMix = 1;



    [Header("3D Sound Settings")]
    [Range(0, 360)]
    public int spread = 0;
    [Range(0f, 5f)]
    public float dopplerLevel = 1;

    [Space(10)]
    public AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;

    [Space(10)]
    public float minDistance = 0;

    [Space(10)]
    public float maxDistance = 500;

    public AnimationCurve RolloffCustomCurve;
    public AnimationCurve SpartialBlendCurve;
    public AnimationCurve SpreadCustomCurve;
    public AnimationCurve ReverbZoneMixCustomCurve;



}



