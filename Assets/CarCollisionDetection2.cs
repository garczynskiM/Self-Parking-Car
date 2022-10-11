using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetection2: MonoBehaviour
{
    CarAgent2 carAgentListener;
    //private readonly string tagObstacle = "Obstacle";
    private readonly string tagTarget = "Target";
    private readonly string tagTargetBounds = "Target Bounds";

    public void Initialize(CarAgent2 carAgent)
    {
        carAgentListener = carAgent;
    }
    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tagObstacle)
        {
            carAgentListener.OnCollisionEnter(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == tagObstacle)
        {
            carAgentListener.OnCollisionStay(collision);
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagTarget)
        {
            carAgentListener.OnTargetEnter(other);
        }
        if(other.gameObject.tag == tagTargetBounds)
        {
            carAgentListener.OnBoundsEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagTarget)
        {
            carAgentListener.OnTargetExit(other);
        }
        if (other.gameObject.tag == tagTargetBounds)
        {
            carAgentListener.OnBoundsExit(other);
        }
    }

}
