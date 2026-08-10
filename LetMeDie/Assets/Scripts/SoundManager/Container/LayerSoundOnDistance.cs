using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSoundOnDistance : MonoBehaviour
{
    public Transform player;
    public float minHearDistance;
    public float maxHearDistance;

    bool trigger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Vector3.Distance(player.transform.position,transform.position) < minHearDistance)
        {
            if (!trigger)
            {
                SoundManager.instance.StartLayerLibarySound(SoundLibary.Music.MainMenu);
                trigger = true;
            }
            float percentageValue = (minHearDistance - Vector3.Distance(player.transform.position, transform.position)) / (minHearDistance-maxHearDistance);
            Debug.Log(percentageValue);
            SoundManager.instance.PlayLayerLibarySound(SoundLibary.Music.MainMenu,percentageValue);
        }
        else
        {
            if (trigger)
            {
                SoundManager.instance.StopLayerLibarySound(SoundLibary.Music.MainMenu);
                trigger = false;
            }
        }
        */
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minHearDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxHearDistance);
    }
}
