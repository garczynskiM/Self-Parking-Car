using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationToggleManager : MonoBehaviour
{
    public void autoRestartToggleChanged(Toggle change)
    {
        SimulationSettingsStaticVars.autoRestart = change.isOn;
    }
    public void otherCarsToggleChanged(Toggle change)
    {
        SimulationSettingsStaticVars.otherCars = change.isOn;
    }
}
