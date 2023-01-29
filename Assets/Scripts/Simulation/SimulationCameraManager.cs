using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SimulationCameraMode
{
    Overhead,
    BehindCar,
}
public class SimulationCameraManager : MonoBehaviour
{
    public void DropdownChangeValue(TMP_Dropdown change)
    {
        SimulationCameraMode cameraMode = (SimulationCameraMode)change.value;
        switch(cameraMode)
        {
            case SimulationCameraMode.Overhead:
                SimulationSettingsSingleton.Instance.overheadCamera.SetActive(true);
                SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(false);
                break;
            case SimulationCameraMode.BehindCar:
                SimulationSettingsSingleton.Instance.overheadCamera.SetActive(false);
                SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(true);
                break;
            default:
                break;
        }
    }
}
