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
        RaceCameraMode cameraMode = (RaceCameraMode)change.value;
        RaceSettingsStaticVars.raceCameraMode = cameraMode;
        switch (RaceSettingsStaticVars.raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                switch (cameraMode)
                {
                    case RaceCameraMode.Overhead:
                        RaceSettingsStaticVars.overheadCamera.SetActive(true);
                        RaceSettingsStaticVars.behindCarCamera.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
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
                    case RaceCameraMode.Overhead:
                        RaceSettingsStaticVars.playerOverheadCamera.SetActive(true);
                        RaceSettingsStaticVars.playerBehindCarCamera.SetActive(false);
                        RaceSettingsStaticVars.modelOverheadCamera.SetActive(true);
                        RaceSettingsStaticVars.modelBehindCarCamera.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
                        RaceSettingsStaticVars.playerOverheadCamera.SetActive(false);
                        RaceSettingsStaticVars.playerBehindCarCamera.SetActive(true);
                        RaceSettingsStaticVars.modelOverheadCamera.SetActive(false);
                        RaceSettingsStaticVars.modelBehindCarCamera.SetActive(true);
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
