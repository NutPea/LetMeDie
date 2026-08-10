using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public MusicSoundContainer musicSoundContainer;
    public SFXSoundContainer SFXSoundContainer;
    public AmbientSoundContainer ambientSoundContainer;
    public List<AudioSource> audioSources;
    public List<AudioSource> mainMusicSources;
    private Sound currentMainMusicSound;
    private SoundLibary.Music currentMainMusic = SoundLibary.Music.None;
    private SoundLibary.Music lastMainMusic = SoundLibary.Music.None;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #region MainMusic

    public void PlayMainMusic(SoundLibary.Music music,float crossfaidTime = 1, int iterations = 20)
    {
        Sound targetSound = FindSound(music);
        lastMainMusic = currentMainMusic;
        currentMainMusic = music;
        //When no music is playing
        if(mainMusicSources[0].clip == null || !mainMusicSources[0].isPlaying)
        {
            targetSound.SetAudioSourceSettings(mainMusicSources[0]);
            targetSound.SetFirstClip(mainMusicSources[0]);
            mainMusicSources[0].loop = true;
            mainMusicSources[0].Play();
            currentMainMusicSound = targetSound;
        }
        else
        {
            if (currentMainMusicSound == targetSound) return;
            //Fades music 
            CrossfadeMainMusic(targetSound,crossfaidTime,iterations);
        }
    }
    public void PlayLastMainMusic()
    {
        if (lastMainMusic == SoundLibary.Music.None) return;
        PlayMainMusic(lastMainMusic);
    }


    public void CrossfadeMainMusic(Sound toSound, float time = 1, int iterations = 20)
    {
        StartCoroutine(CrossfadeMainMusicOverTime( toSound, time, iterations));
    }
    IEnumerator CrossfadeMainMusicOverTime(Sound toSound, float time, int iterations)
    {
        float iterationtime = time / iterations;
        AudioSource toAudioSource = FindFreeMainMusicAudioSource();
        toSound.SetFirstClip(toAudioSource);
        AudioSource fromAudioSource = mainMusicSources[0];

        toSound.SetFirstAudioSource(toAudioSource);
        toSound.SetAudioSourceSettings(toAudioSource);
        toAudioSource.Play();

        for (int i = 1; i <= iterations; i++)
        {
            toAudioSource.volume = toSound.audioSetting.volume * ((float)i / iterations);
            fromAudioSource.volume = currentMainMusicSound.audioSetting.volume * (1 - (float)i / iterations);
            yield return new WaitForSeconds(iterationtime);
        }

        int toAudioSourceIndex = mainMusicSources.IndexOf(toAudioSource);
        mainMusicSources[0] = toAudioSource;
        mainMusicSources[toAudioSourceIndex] = fromAudioSource;

        fromAudioSource.Stop();
        fromAudioSource.clip = null;


        currentMainMusicSound = toSound;

        yield return null;
    }

    #endregion

    #region LayeredSound
    public void StartLayerLibarySound(SoundLibary.Music music)
    {
       //Finds and Occupies AudioSources
       Sound foundSound = FindSound(music);
        StartLayerSound(foundSound);
    }
    public void StopLayerLibarySound(SoundLibary.Music music)
    {
        //Finds and Gives AudioSourcesFree
        Sound foundSound = FindSound(music);
        StopLayerSound(foundSound);
    }
    public void PlayLayerLibarySound(SoundLibary.Music music, float percentageValue)
    {
        Sound foundSound = FindSound(music);
        PlayLayerSound(foundSound, percentageValue);
    }

    public void StartLayerLibarySound(SoundLibary.Ambient ambient)
    {
        //Finds and Occupies AudioSources
        Sound foundSound = FindSound(ambient);
        StartLayerSound(foundSound);
    }
    public void StopLayerLibarySound(SoundLibary.Ambient ambient)
    {
        //Finds and Gives AudioSourcesFree
        Sound foundSound = FindSound(ambient);
        StopLayerSound(foundSound);
    }
    public void PlayLayerLibarySound(SoundLibary.Ambient ambient, float percentageValue)
    {
        Sound foundSound = FindSound(ambient);
        PlayLayerSound(foundSound, percentageValue);
    }


    /// <summary>
    /// Occupies audiosources 
    /// </summary>
    public void StartLayerSound(Sound sound)
    {
        for (int i = 0; i < sound.soundClips.Count; i++)
        {
            AudioSource audioSource = FindFreeAudioSource();
            sound.SetAudioSourceSettings(audioSource);
            audioSource.clip = sound.soundClips[i].audioClip;
            audioSource.loop = true;
            audioSource.Play();
            audioSource.volume = 0;
            sound.usedAudioSources.Add(audioSource);
        }
    }
    /// <summary>
    /// Gives occupied audiosources free 
    /// </summary>
    public void StopLayerSound(Sound sound)
    {
        for (int i = 0; i < sound.soundClips.Count; i++)
        {
            AudioSource audioSource = sound.usedAudioSources[i];
            audioSource.Stop();
            audioSource.clip = null;
        }
        sound.usedAudioSources.Clear();
    }
    /// <summary>
    /// Layers Sound before that you need to start and stop target layering sound
    /// </summary>
    public void PlayLayerSound(Sound sound,float percentageValue)
    {
        sound.SetLayeredAudioSourceVolume(percentageValue);
    }




    #endregion

    #region CrossFade
    public void CrossfadeLibarySound(SoundLibary.Music toMusic, SoundLibary.Music fromMusic, float time = 1, int iterations = 20)
    {
        Sound fromSound = FindSound(toMusic);
        Sound toSound = FindSound(fromMusic);

        CrossfadeSounds(fromSound, toSound, time, iterations);
    }

    public void CrossfadeLibarySound(SoundLibary.Ambient fromAmbient, SoundLibary.Ambient toAmbient, float time = 1, int iterations = 20)
    {
        Sound fromSound = FindSound(fromAmbient);
        Sound toSound = FindSound(toAmbient);

        CrossfadeSounds(fromSound, toSound, time,iterations);
    }

    public void CrossfadeSounds(Sound fromSound,Sound toSound,float time = 1,int iterations = 20)
    {
        StartCoroutine(CrossfadeOverTime(fromSound, toSound, time, iterations));
    }

    IEnumerator CrossfadeOverTime(Sound fromSound, Sound toSound, float time, int iterations)
    {
        float iterationtime = time / iterations;
        AudioSource toAudioSource = FindFreeAudioSource();
        toSound.SetFirstClip(toAudioSource);
        AudioSource fromAudioSource = fromSound.usedAudioSources[0];

        toSound.SetFirstAudioSource(toAudioSource);
        toSound.SetAudioSourceSettings(toAudioSource);
        toAudioSource.Play();

        for (int i = 1; i <= iterations; i++)
        {
            toAudioSource.volume = toSound.audioSetting.volume * ((float)i / iterations);
            fromAudioSource.volume = fromSound.audioSetting.volume * (1 - (float)i / iterations);
            yield return new WaitForSeconds(iterationtime);
        }

        fromAudioSource.Stop();
        fromAudioSource.clip = null;
       yield return null;
    }

    #endregion

    #region PlaySimpleSoundAtPosition
    /// <summary>
    /// Finds and sets audiosource Settings deppending on the Sound. After that the sound gets played
    /// Sets audiosource on position
    /// </summary>
    public void PlayLibarySoundAtPosition(SoundLibary.Music music,Vector3 position)
    {
        Sound targetSound = FindSound(music);
        PlaySoundAtPosition(targetSound, position);
    }

    /// <summary>
    /// Finds and sets audiosource Settings deppending on the Sound. After that the sound gets played
    /// Sets audiosource on position
    /// </summary>
    public void PlayLibarySoundAtPosition(SoundLibary.SFX sfx, Vector3 position)
    {
        Sound targetSound = FindSound(sfx);
        PlaySoundAtPosition(targetSound, position);
    }

    /// <summary>
    /// Finds and sets audiosource Settings deppending on the Sound. After that the sound gets played
    /// Sets audiosource on position
    /// </summary>
    public void PlayLibarySoundAtPosition(SoundLibary.Ambient ambient, Vector3 position)
    {
        Sound targetSound = FindSound(ambient);
        PlaySoundAtPosition(targetSound, position);
    }

    /// <summary>
    /// Plays Sound on position . Used for 3D Sounds.
    /// </summary>
    public void PlaySoundAtPosition(Sound sound , Vector3 position)
    {
        AudioSource audioSource = FindFreeAudioSource();
        sound.SetAudioSourceSettings(audioSource);
        sound.SetFirstClip(audioSource);
        audioSource.transform.position = position;
        audioSource.enabled = true;
        audioSource.Play();
    }

    public void PlayRandomeSoundAtPosition(Sound sound,Vector3 position)
    {
        AudioSource audioSource = FindFreeAudioSource();
        sound.SetAudioSourceSettings(audioSource);
        sound.SetRandomClipToAudioSource(audioSource);
        audioSource.transform.position = position;
        audioSource.enabled = true;
        audioSource.Play();
    }

    #endregion

    #region PlayRandomContinuse

    public void PlayRandomContinuousLibarySound(SoundLibary.SFX sFX)
    {
        Sound sound = FindSound(sFX);
        PlayRandomContinuousSound(sound);
    }

    public void PlayRandomContinuousSound(Sound sound)
    {
        if (sound.canRepeatItself)
        {
            sound.RemoveEmptysInUsedAudioSource();
            if (sound.usedAudioSources.Count == 0)
            {
                AudioSource audioSource = FindFreeAudioSource();
                sound.usedAudioSources.Add(audioSource);
            }
            else if (sound.usedAudioSources[0].isPlaying)
            {

                AudioSource audioSource = FindFreeAudioSource();
                sound.usedAudioSources[0] = audioSource;

            }

            sound.SetAudioSourceSettings(sound.usedAudioSources[0]);
            sound.SetRandomClipToAudioSource(sound.usedAudioSources[0]);
            sound.usedAudioSources[0].Play();
            sound.canRepeatItself = false;
        }
        else if (!sound.inCouroutine)
        {
            sound.inCouroutine = true;
            StartCoroutine(PlaySoundRoutine(sound.repeatTimer, sound));
        }
    }

    #endregion

    #region PlayContinouse

    /// <summary>
    /// Repeats Libarysound after sound repeatTimer
    /// </summary>
    public void PlayContinuousLibarySound(SoundLibary.SFX sFX)
    {
        Sound sound = FindSound(sFX);
        PlayContinuousSound(sound);
    }

    public void PlayContinuousSound(Sound sound)
    {
        if (sound.canRepeatItself)
        {
            sound.RemoveEmptysInUsedAudioSource();
            if (sound.usedAudioSources.Count == 0)
            {
                AudioSource audioSource = FindFreeAudioSource();
                sound.usedAudioSources.Add(audioSource);
            }
            else if (sound.usedAudioSources[0].isPlaying)
            {
                AudioSource audioSource = FindFreeAudioSource();
                sound.usedAudioSources[0] = audioSource;

            }

            sound.SetAudioSourceSettings(sound.usedAudioSources[0]);
            sound.SetFirstClip(sound.usedAudioSources[0]);
            sound.usedAudioSources[0].Play();
            sound.canRepeatItself = false;
        }
        else if (!sound.inCouroutine)
        {
            sound.inCouroutine = true;
            StartCoroutine(PlaySoundRoutine(sound.repeatTimer, sound));
        }
    }

    IEnumerator PlaySoundRoutine(float time, Sound sound)
    {
        yield return new WaitForSeconds(time);
        sound.inCouroutine = false;
        sound.canRepeatItself = true;
    }
    #endregion

    #region PlayRandomSound
    /// <summary>
    /// Finds and plays random sound from libary
    /// </summary>
    public void PlayRandomLibarySound(SoundLibary.Music music)
    {
        Sound targetSound = FindSound(music);
        PlayRandomSound(targetSound);
    }
    /// <summary>
    /// Finds and plays random sound from libary
    /// </summary>
    public void PlayRandomLibarySound(SoundLibary.SFX sfx)
    {
        Sound targetSound = FindSound(sfx);
        PlayRandomSound(targetSound);
    }
    /// <summary>
    /// Finds and plays random sound from libary
    /// </summary>
    public void PlayRandomLibarySound(SoundLibary.Ambient ambient)
    {
        Sound targetSound = FindSound(ambient);
        PlayRandomSound(targetSound);
    }
    /// <summary>
    /// Finds and sets audiosource Settings deppending on the Sound.After that a randome clip will be played
    /// </summary>
    public void PlayRandomSound(Sound sound)
    {
        AudioSource audioSource = FindFreeAudioSource();
        sound.SetAudioSourceSettings(audioSource);
        sound.SetRandomClipToAudioSource(audioSource);
        sound.SetFirstAudioSource(audioSource);
        audioSource.enabled = true;
        audioSource.Play();
    }

    #endregion

    #region PlaySimpleSounds
    /// <summary>
    /// Finds and plays sound from libary
    /// </summary>
    public void PlayLibarySound(SoundLibary.Music music)
    {
        Sound targetSound = FindSound(music);
        PlaySound(targetSound);
    }
    /// <summary>
    /// Finds and plays sound from libary
    /// </summary>
    public void PlayLibarySound(SoundLibary.SFX sfx)
    {
        Sound targetSound = FindSound(sfx);
        PlaySound(targetSound);
    }
    /// <summary>
    /// Finds and plays sound from libary
    /// </summary>
    public void PlayLibarySound(SoundLibary.Ambient ambient)
    {
        Sound targetSound = FindSound(ambient);
        PlaySound(targetSound);
    }

    /// <summary>
    /// Finds and sets audiosource Settings deppending on the Sound.After that the sound gets played
    /// </summary>
    public void  PlaySound(Sound sound)
    {
        AudioSource audioSource = FindFreeAudioSource();
        sound.SetAudioSourceSettings(audioSource);
        sound.SetFirstClip(audioSource);
        sound.SetFirstAudioSource(audioSource);
        audioSource.enabled = true;
        audioSource.Play();
    }
    #endregion

    #region FindSounds
    public Sound FindSound(SoundLibary.Music music)
    {
        return musicSoundContainer.FindSound(music);
    }

    public Sound FindSound(SoundLibary.SFX sfx)
    {
        return SFXSoundContainer.FindSound(sfx);
    }

    public Sound FindSound(SoundLibary.Ambient ambient)
    {
        return ambientSoundContainer.FindSound(ambient);
    }
    #endregion

    #region FindFreeAudioSource
    /// <summary>
    /// Finds free AudioSource
    /// </summary>
    private AudioSource FindFreeAudioSource()
    {
        return FindAvaibleAudioSourceInList(audioSources);
    }
    private AudioSource FindFreeMainMusicAudioSource()
    {
        return FindAvaibleAudioSourceInList(mainMusicSources);
    }
    private AudioSource FindAvaibleAudioSourceInList(List<AudioSource> audioSources)
    {
        AudioSource targetAudioSource = null;
        foreach (AudioSource audioSource in audioSources)
        {
            AudioClip clip = audioSource.clip;
            if (clip == null || !audioSource.isPlaying)
            {
                targetAudioSource = audioSource;
                break;
            }
        }

        if (targetAudioSource == null)
        {
            GameObject audioSourceGameobject = new GameObject("Added AudioSource");
            audioSourceGameobject.transform.parent = transform.GetChild(0);
            targetAudioSource = audioSourceGameobject.AddComponent<AudioSource>();
            audioSources.Add(targetAudioSource);
        }
        targetAudioSource.enabled = true;
        return targetAudioSource;
    }


    #endregion

}
