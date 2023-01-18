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
                RaceSettingsSingleton.Instance.playerOverheadCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.playerBehindCarCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.modelOverheadCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.modelBehindCarCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.playerParking.SetActive(false);
                RaceSettingsSingleton.Instance.modelParking.SetActive(false);
                RaceSettingsSingleton.Instance.sequentialParking.SetActive(true);
                switch (RaceSettingsSingleton.Instance.raceCameraMode)
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
                RaceSettingsSingleton.Instance.parkingGenerated = false;
                RaceSettingsSingleton.Instance.generateParkingSlots();
                RaceSettingsSingleton.Instance.overheadCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.behindCarCamera.gameObject.SetActive(false);
                RaceSettingsSingleton.Instance.sequentialParking.SetActive(false);
                RaceSettingsSingleton.Instance.playerParking.SetActive(true);
                RaceSettingsSingleton.Instance.modelParking.SetActive(true);
                switch (RaceSettingsSingleton.Instance.raceCameraMode)
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
