using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadRaceCamerasAndMaps : MonoBehaviour
{
    [SerializeField] private Transform m_sequentialParking;
    [SerializeField] private Transform m_overheadCamera;
    [SerializeField] private Transform m_behindCarCamera;
    [SerializeField] private Transform m_playerParking;
    [SerializeField] private Transform m_playerOverheadCamera;
    [SerializeField] private Transform m_playerBehindCarCamera;
    [SerializeField] private Transform m_modelParking;
    [SerializeField] private Transform m_modelOverheadCamera;
    [SerializeField] private Transform m_modelBehindCarCamera;
    // Start is called before the first frame update
    void Start()
    {
        RaceSettingsStaticVars.sequentialParking = m_sequentialParking.gameObject;
        RaceSettingsStaticVars.overheadCamera = m_overheadCamera.gameObject;
        RaceSettingsStaticVars.behindCarCamera = m_behindCarCamera.gameObject;
        RaceSettingsStaticVars.playerParking = m_playerParking.gameObject;
        RaceSettingsStaticVars.playerOverheadCamera = m_playerOverheadCamera.gameObject;
        RaceSettingsStaticVars.playerBehindCarCamera = m_playerBehindCarCamera.gameObject;
        RaceSettingsStaticVars.modelParking = m_modelParking.gameObject;
        RaceSettingsStaticVars.modelOverheadCamera = m_modelOverheadCamera.gameObject;
        RaceSettingsStaticVars.modelBehindCarCamera = m_modelBehindCarCamera.gameObject;
        activateRelevantObjects();
    }
    private void activateRelevantObjects()
    {
        switch (RaceSettingsStaticVars.raceOrder)
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
