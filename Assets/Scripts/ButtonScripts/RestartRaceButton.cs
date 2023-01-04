using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartRaceButton : MonoBehaviour
{
    public void restartRace()
    {
        var car = GameObject.Find("car-root");
        RaceSettingsStaticVars.manualRestart = true;
        if (RaceSettingsStaticVars.raceOrder == RaceOrder.FirstPlayerThenModel)
        {
            var script = car.GetComponent<SequentialRaceCarAgent>();
            script.EndEpisode();
        }
    }
}
