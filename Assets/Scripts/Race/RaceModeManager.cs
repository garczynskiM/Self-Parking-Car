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
        RaceSettingsStaticVars.raceOrder = raceOrder;
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                RaceSettingsStaticVars.playerOverheadCamera.SetActive(false);
                RaceSettingsStaticVars.playerBehindCarCamera.SetActive(false);
                RaceSettingsStaticVars.modelOverheadCamera.SetActive(false);
                RaceSettingsStaticVars.modelBehindCarCamera.SetActive(false);
                RaceSettingsStaticVars.playerParking.SetActive(false);
                RaceSettingsStaticVars.modelParking.SetActive(false);
                RaceSettingsStaticVars.sequentialParking.SetActive(true);
                switch (RaceSettingsStaticVars.raceCameraMode)
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
                RaceSettingsStaticVars.parkingGenerated = false;
                RaceSettingsStaticVars.generateParkingSlots();
                RaceSettingsStaticVars.overheadCamera.SetActive(false);
                RaceSettingsStaticVars.behindCarCamera.SetActive(false);
                RaceSettingsStaticVars.sequentialParking.SetActive(false);
                RaceSettingsStaticVars.playerParking.SetActive(true);
                RaceSettingsStaticVars.modelParking.SetActive(true);
                switch (RaceSettingsStaticVars.raceCameraMode)
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
