using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceToggleManager : MonoBehaviour
{
    [SerializeField] public Slider startPositionSlider;
    [SerializeField] public Slider endPositionSlider;
    public void otherCarsToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.otherCars = change.isOn;
    }
    public void setStartToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.setStart = change.isOn;
        startPositionSlider.interactable = change.isOn;
    }
    public void setEndToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.setEnd = change.isOn;
        endPositionSlider.interactable = change.isOn;
    }
}
