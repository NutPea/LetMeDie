using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundHolder : MonoBehaviour
{
    public PlayerStepHandler.StepUnderground stepUnderground;
    private PlayerStepHandler stepHandler;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(stepHandler == null)
            {
                stepHandler = collision.gameObject.GetComponent<PlayerStepHandler>();
            }
            stepHandler.SetUnderground(stepUnderground);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (stepHandler == null)
            {
                stepHandler = other.gameObject.GetComponent<PlayerStepHandler>();
            }
            stepHandler.SetUnderground(stepUnderground);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (stepHandler == null)
            {
                stepHandler = collision.gameObject.GetComponent<PlayerStepHandler>();
            }
            stepHandler.ResetUnderground();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (stepHandler == null)
            {
                stepHandler = other.gameObject.GetComponent<PlayerStepHandler>();
            }
            stepHandler.SetUnderground(stepUnderground);
        }
    }
}
