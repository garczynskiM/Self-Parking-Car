using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceToggleManager : MonoBehaviour
{
    public void otherCarsToggleChanged(Toggle change)
    {
        RaceSettingsSingleton.Instance.otherCars = change.isOn;
    }
}
