using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceModeManager : MonoBehaviour
{
    public void DropdownChangeValue(TMP_Dropdown change)
    {
        RaceOrder raceOrder = (RaceOrder)change.value;
        RaceSettingsSingleton.Instance.raceOrder = raceOrder;
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                RaceSettingsSingleton.Instance.playerOverheadCamera.SetActive(false);
                RaceSettingsSingleton.Instance.playerBehindCarCamera.SetActive(false);
                RaceSettingsSingleton.Instance.modelOverheadCamera.SetActive(false);
                RaceSettingsSingleton.Instance.modelBehindCarCamera.SetActive(false);
                RaceSettingsSingleton.Instance.playerParking.SetActive(false);
                RaceSettingsSingleton.Instance.modelParking.SetActive(false);
                RaceSettingsSingleton.Instance.sequentialParking.SetActive(true);
                switch (RaceSettingsSingleton.Instance.raceCameraMode)
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
                RaceSettingsSingleton.Instance.parkingGenerated = false;
                RaceSettingsSingleton.Instance.generateParkingSlots();
                RaceSettingsSingleton.Instance.overheadCamera.SetActive(false);
                RaceSettingsSingleton.Instance.behindCarCamera.SetActive(false);
                RaceSettingsSingleton.Instance.sequentialParking.SetActive(false);
                RaceSettingsSingleton.Instance.playerParking.SetActive(true);
                RaceSettingsSingleton.Instance.modelParking.SetActive(true);
                switch (RaceSettingsSingleton.Instance.raceCameraMode)
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
