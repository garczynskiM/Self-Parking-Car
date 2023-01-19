using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationToggleManager : MonoBehaviour
{
    [SerializeField] public Slider startPositionSlider;
    [SerializeField] public Slider endPositionSlider;
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
        startPositionSlider.interactable = change.isOn;
    }
    public void setEndToggleChanged(Toggle change)
    {
        SimulationSettingsSingleton.Instance.setEnd = change.isOn;
        endPositionSlider.interactable = change.isOn;
    }
}
