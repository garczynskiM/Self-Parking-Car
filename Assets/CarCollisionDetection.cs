using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetection : MonoBehaviour
{
    CarAgent carAgentListener;
    string tagName = "Obstacle";

    public void Initialize(CarAgent carAgent)
    {
        carAgentListener = carAgent;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == tagName)
        {
            carAgentListener.OnCollisionEnter(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == tagName)
        {
            carAgentListener.OnCollisionExit(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == tagName)
        {
            carAgentListener.OnCollisionStay(collision);
        }
    }

}
