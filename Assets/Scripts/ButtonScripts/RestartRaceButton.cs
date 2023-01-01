using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartRaceButton : MonoBehaviour
{
    public void restartRace()
    {
        var car = GameObject.Find("car-root");
        var script = car.GetComponent<SimulationCarAgent>();
        SimulationSettingsStaticVars.manualRestart = true;
        script.EndEpisode();
    }
}
