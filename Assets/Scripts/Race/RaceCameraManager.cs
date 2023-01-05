using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RaceCameraMode
{
    Overhead,
    BehindCar,
}
public class RaceCameraManager : MonoBehaviour
{
    public void DropdownChangeValue(TMP_Dropdown change)
    {
        //if (!camerasInit) InitCameras();
        SimulationCameraMode cameraMode = (SimulationCameraMode)change.value;
        switch (RaceSettingsStaticVars.raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                switch (cameraMode)
                {
                    case SimulationCameraMode.Overhead:
                        RaceSettingsStaticVars.overheadCamera.SetActive(true);
                        RaceSettingsStaticVars.behindCarCamera.SetActive(false);
                        break;
                    case SimulationCameraMode.BehindCar:
                        RaceSettingsStaticVars.overheadCamera.SetActive(false);
                        RaceSettingsStaticVars.behindCarCamera.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case RaceOrder.PlayerAndModel:
                switch (cameraMode)
                {
                    case SimulationCameraMode.Overhead:
                        break;
                    case SimulationCameraMode.BehindCar:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        
    }
}
