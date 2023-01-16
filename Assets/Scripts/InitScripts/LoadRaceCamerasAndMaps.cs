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
    [SerializeField] private Transform m_sequentialCarMarker;
    [SerializeField] private Transform m_playerParking;
    [SerializeField] private Transform m_playerOverheadCamera;
    [SerializeField] private Transform m_playerBehindCarCamera;
    [SerializeField] private Transform m_playerCarMarker;
    [SerializeField] private Transform m_modelParking;
    [SerializeField] private Transform m_modelOverheadCamera;
    [SerializeField] private Transform m_modelBehindCarCamera;
    [SerializeField] private Transform m_modelCarMarker;
    [SerializeField] public float minimumZ;
    [SerializeField] public float maximumZ;
    // Start is called before the first frame update
    void Start()
    {
        RaceSettingsSingleton.Instance.sequentialParking = m_sequentialParking.gameObject;
        RaceSettingsSingleton.Instance.overheadCamera = m_overheadCamera.gameObject;
        RaceSettingsSingleton.Instance.behindCarCamera = m_behindCarCamera.gameObject;
        RaceSettingsSingleton.Instance.playerParking = m_playerParking.gameObject;
        RaceSettingsSingleton.Instance.playerOverheadCamera = m_playerOverheadCamera.gameObject;
        RaceSettingsSingleton.Instance.playerBehindCarCamera = m_playerBehindCarCamera.gameObject;
        RaceSettingsSingleton.Instance.modelParking = m_modelParking.gameObject;
        RaceSettingsSingleton.Instance.modelOverheadCamera = m_modelOverheadCamera.gameObject;
        RaceSettingsSingleton.Instance.modelBehindCarCamera = m_modelBehindCarCamera.gameObject;

        RaceSettingsSingleton.Instance.sequentialStartMarker = m_sequentialCarMarker;
        RaceSettingsSingleton.Instance.playerStartMarker = m_playerCarMarker;
        RaceSettingsSingleton.Instance.modelStartMarker = m_modelCarMarker;
        RaceSettingsSingleton.Instance.minimumZ = minimumZ;
        RaceSettingsSingleton.Instance.maximumZ = maximumZ;
        RaceSettingsSingleton.Instance.updateSetStartSlider();

        activateRelevantObjects();
    }
    private void activateRelevantObjects()
    {
        RaceSettingsSingleton.Instance.playerStartMarker.gameObject.SetActive(false);
        RaceSettingsSingleton.Instance.modelStartMarker.gameObject.SetActive(false);
        RaceSettingsSingleton.Instance.sequentialStartMarker.gameObject.SetActive(false);
        switch (RaceSettingsSingleton.Instance.raceOrder)
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
