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
        RaceSettingsSingleton.Instance.raceCameraMode = cameraMode;
        switch (RaceSettingsSingleton.Instance.raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                switch (cameraMode)
                {
                    case RaceCameraMode.Overhead:
                        RaceSettingsSingleton.Instance.overheadCamera.SetActive(true);
                        RaceSettingsSingleton.Instance.behindCarCamera.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
                        RaceSettingsSingleton.Instance.overheadCamera.SetActive(false);
                        RaceSettingsSingleton.Instance.behindCarCamera.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case RaceOrder.PlayerAndModel:
                switch (cameraMode)
                {
                    case RaceCameraMode.Overhead:
                        RaceSettingsSingleton.Instance.playerOverheadCamera.SetActive(true);
                        RaceSettingsSingleton.Instance.playerBehindCarCamera.SetActive(false);
                        RaceSettingsSingleton.Instance.modelOverheadCamera.SetActive(true);
                        RaceSettingsSingleton.Instance.modelBehindCarCamera.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
                        RaceSettingsSingleton.Instance.playerOverheadCamera.SetActive(false);
                        RaceSettingsSingleton.Instance.playerBehindCarCamera.SetActive(true);
                        RaceSettingsSingleton.Instance.modelOverheadCamera.SetActive(false);
                        RaceSettingsSingleton.Instance.modelBehindCarCamera.SetActive(true);
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
