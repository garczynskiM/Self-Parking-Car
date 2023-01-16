using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationToggleManager : MonoBehaviour
{
    [SerializeField] public Slider slider;
    public void autoRestartToggleChanged(Toggle change)
    {
        SimulationSettingsSingleton.Instance.autoRestart = change.isOn;
    }
    public void otherCarsToggleChanged(Toggle change)
    {
        SimulationSettingsSingleton.Instance.otherCars = change.isOn;
    }
    public void setStartToggleChanged(Toggle change)
    {
        SimulationSettingsSingleton.Instance.setStart = change.isOn;
        slider.interactable = change.isOn;
    }
}
