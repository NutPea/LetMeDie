using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveUIHandler : MonoBehaviour
{
    [SerializeField] private WaveHandler waveHandler;
    [SerializeField] private List<WaveIndexHandler> lastWaveIndecies = new();
    [SerializeField] private WaveIndexHandler currentWaveIndex;
    [SerializeField] private List<WaveIndexHandler> nextWaveIndecies = new();

    [SerializeField] private int shownWaveIndecies = 3;

    private void Start()
    {
        DrawWaves();
        waveHandler.OnWaveChange.AddListener(OnWaveChange);
    }

    private void OnWaveChange(int arg0, Wave arg1)
    {
        DrawWaves();
    }

    private void DrawWaves()
    {
        int beforeWavesIteration = waveHandler.CurrentWaveIndex - shownWaveIndecies;
        for (int i = 0; i < shownWaveIndecies; i++) {
            int index = beforeWavesIteration + i;
            if(index >= 0)
            {
                lastWaveIndecies[i].SetWave(waveHandler.Waves[index]);
            }
            else
            {
                lastWaveIndecies[i].SetNothing();
            }
        }

        currentWaveIndex.SetWave(waveHandler.CurrentWave);

        for (int i = 0; i < shownWaveIndecies; i++) {
            int index = waveHandler.CurrentWaveIndex + i+1;
            if (index >= waveHandler.Waves.Count) {
                nextWaveIndecies[i].SetNothing();
                continue;
            }
            nextWaveIndecies[i].SetWave(waveHandler.Waves[index]);
        }
    }

}
