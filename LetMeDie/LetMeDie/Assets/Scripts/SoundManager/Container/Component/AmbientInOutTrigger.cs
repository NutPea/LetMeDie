using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientInOutTrigger : MonoBehaviour
{
    public SoundLibary.Ambient inAmbientSound;
    public SoundLibary.Ambient goOutAmbientSound;
    public float percentageValue = 1;

    private bool playerIsInBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
          //  SoundManager.instance.PlayLayerLibarySound(ambientSound,percentageValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
          //  SoundManager.instance.StopLayerLibarySound(ambientSound);
        }
    }
}
