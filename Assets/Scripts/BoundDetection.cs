using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundDetection : MonoBehaviour
{
    CarAgent2 carAgentListener;
    private readonly string tagAgent = "Agent";

    public void Initialize(CarAgent2 carAgent)
    {
        carAgentListener = carAgent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagAgent)
        {
            carAgentListener.OnBoundsEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == tagAgent)
        {
            carAgentListener.OnBoundsExit(other);
        }
    }


}

