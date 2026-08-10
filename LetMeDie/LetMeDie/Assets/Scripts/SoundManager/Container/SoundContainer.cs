using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

public class SoundContainer : ScriptableObject
{
    public AudioMixerGroup defaultMixerGroup;
    [NonReorderable]
    public List<Sound> sounds = new List<Sound>();

    public void UpdateSoundList(Type enumType)
    {
        DeleteNotNeededSounds(enumType);
        AddNewSounds(enumType);
        RenameSounds(enumType);
        //OrderList(enumType);
    }

    private void RenameSounds(Type enumType)
    {
        for (int i = 0; i < Enum.GetValues(enumType).Length - 1; i++)
        {
            String enumSoundName = Enum.GetName(enumType, i + 1);
            String soundName = sounds[i].name;
            if (enumSoundName != soundName)
            {
                sounds[i].name = enumSoundName;
            }
            continue;
        }
    }

    private void AddNewSounds(Type enumType)
    {
        if (Enum.GetValues(enumType).Length - 1 == sounds.Count) return;
        List<Sound> newSoundList = new List<Sound>();
        int addedCounter = 0;
        for (int i = 0; i < Enum.GetValues(enumType).Length-1; i++)
        {
            String soundName = Enum.GetName(enumType,i+1);
            if (CheckIfSoundExisted(soundName))
            {
                newSoundList.Add(sounds[i - addedCounter]);
                continue;
            }
            Sound sound = new Sound();
            sound.name = soundName;
            newSoundList.Add(sound);
            addedCounter++;
        }
        sounds.Clear();
        sounds = newSoundList;
    }

    private bool CheckIfSoundExisted(string soundName)
    {
        bool soundExisted = false;
        foreach (Sound sound in sounds)
        {
            if (sound.name == soundName)
            {
                soundExisted = true;
                break;
            }
        }

        return soundExisted;
    }

    private void DeleteNotNeededSounds(Type enumType)
    {
        if (Enum.GetValues(enumType).Length-1 > sounds.Count) return;
        List<Sound> newSounds = sounds;
        List<string> songNames = new List<string>();
        for (int i = 0; i < sounds.Count; i++)
        {
            songNames.Add(sounds[i].name);
        }

        List<string> enumNames = new List<string>();
        for (int i = 0; i < Enum.GetValues(enumType).Length - 1; i++)
        {
            enumNames.Add(Enum.GetName(enumType, i + 1));
        }

        List<string> deletedEnums = songNames.Except(enumNames).ToList();
        for (int i = 0; i < deletedEnums.Count; i++)
        {
            Sound wantedSound = new Sound();
            foreach(Sound sound in sounds)
            {
                if (sound.name == deletedEnums[i]) wantedSound = sound;
            }
            newSounds.Remove(wantedSound);
        }

        sounds = newSounds;
    }

    private void OrderList(Type enumType)
    {
        Sound[] orderedArray = new Sound[sounds.Count];

        for (int i = 0; i < sounds.Count; i++)
        {
            for (int j = 0; j < Enum.GetValues(enumType).Length-1; j++)
            {
                String soundName = Enum.GetName(enumType, j+1);
                if (sounds[i].name == soundName)
                {
                    orderedArray[j] = sounds[i];
                }
            }

        }

        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i] = orderedArray[i];
        }
    }

}
