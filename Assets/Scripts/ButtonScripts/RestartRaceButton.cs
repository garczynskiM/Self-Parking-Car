using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartRaceButton : MonoBehaviour
{
    private Toggle m_otherCarsToggle;
    public void restartRace()
    {
        m_otherCarsToggle = MapLoadVarsSingleton.Instance.m_otherCarsTransform.GetComponentInChildren<Toggle>();
        RaceSettingsSingleton.Instance.otherCars = m_otherCarsToggle.isOn;
        if (RaceSettingsSingleton.Instance.raceOrder == RaceOrder.FirstPlayerThenModel)
        {
            RaceSettingsSingleton.Instance.manualRestart = true;
            var car = GameObject.Find("car-root");
            var script = car.GetComponent<SequentialRaceCarAgent>();
            script.EndEpisode();
        }
        else if (RaceSettingsSingleton.Instance.raceOrder == RaceOrder.PlayerAndModel)
        {
            RaceSettingsSingleton.Instance.carManualRestart = true;
            RaceSettingsSingleton.Instance.playerManualRestart = true;
            RaceSettingsSingleton.Instance.parkingGenerated = false;
            RaceSettingsSingleton.Instance.generateParkingSlots();

            var playerCar = GameObject.Find("playerCar");
            if(playerCar != null)
            {
                var playerScript = playerCar.GetComponent<PlayerRaceCarAgent>();
                playerScript.EndEpisode();
            }
            //They won't be both null at the same time - that case is handled by displaying the summary
            var modelCar = GameObject.Find("modelCar");
            if(modelCar != null)
            {
                var modelScript = modelCar.GetComponent<ModelRaceCarAgent>();
                modelScript.EndEpisode();
            }
        }
    }
}
