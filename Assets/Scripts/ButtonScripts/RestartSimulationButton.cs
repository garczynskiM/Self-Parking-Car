using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RestartSimulationButton : MonoBehaviour
{
    public void restartSimulation()
    {
        var car = GameObject.Find("car-root");
        var script = car.GetComponent<SimulationCarAgent>();
        SimulationSettingsSingleton.Instance.manualRestart = true;
        script.EndEpisode();
    }
}
