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
                        RaceSettingsSingleton.Instance.overheadCamera.gameObject.SetActive(true);
                        RaceSettingsSingleton.Instance.behindCarCamera.gameObject.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
                        RaceSettingsSingleton.Instance.overheadCamera.gameObject.SetActive(false);
                        RaceSettingsSingleton.Instance.behindCarCamera.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case RaceOrder.PlayerAndModel:
                switch (cameraMode)
                {
                    case RaceCameraMode.Overhead:
                        RaceSettingsSingleton.Instance.playerOverheadCamera.gameObject.SetActive(true);
                        RaceSettingsSingleton.Instance.playerBehindCarCamera.gameObject.SetActive(false);
                        RaceSettingsSingleton.Instance.modelOverheadCamera.gameObject.SetActive(true);
                        RaceSettingsSingleton.Instance.modelBehindCarCamera.gameObject.SetActive(false);
                        break;
                    case RaceCameraMode.BehindCar:
                        RaceSettingsSingleton.Instance.playerOverheadCamera.gameObject.SetActive(false);
                        RaceSettingsSingleton.Instance.playerBehindCarCamera.gameObject.SetActive(true);
                        RaceSettingsSingleton.Instance.modelOverheadCamera.gameObject.SetActive(false);
                        RaceSettingsSingleton.Instance.modelBehindCarCamera.gameObject.SetActive(true);
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
