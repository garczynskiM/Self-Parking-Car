using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceToggleManager : MonoBehaviour
{
    [SerializeField] public Slider slider;
    public void otherCarsToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.otherCars = change.isOn;
    }
    public void setStartToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.setStart = change.isOn;
        slider.interactable = change.isOn;
    }
}
