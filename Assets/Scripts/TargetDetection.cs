using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    AbstractCarAgent carAgentListener;
    private readonly string tagAgent = "Agent";

    public void Initialize(AbstractCarAgent carAgent)
    {
        carAgentListener = carAgent;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagAgent)
        {
            carAgentListener.OnTargetEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagAgent)
        {
            carAgentListener.OnTargetExit(other);
        }
    }

}
