using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadRaceCamerasAndMaps : MonoBehaviour
{
    [SerializeField] private Transform m_sequentialParking;
    [SerializeField] private Camera m_overheadCamera;
    [SerializeField] private Camera m_behindCarCamera;
    [SerializeField] private Transform m_sequentialCarMarker;
    [SerializeField] private Transform m_playerParking;
    [SerializeField] private Camera m_playerOverheadCamera;
    [SerializeField] private Camera m_playerBehindCarCamera;
    [SerializeField] private Transform m_playerCarMarker;
    [SerializeField] private Transform m_modelParking;
    [SerializeField] private Camera m_modelOverheadCamera;
    [SerializeField] private Camera m_modelBehindCarCamera;
    [SerializeField] private Transform m_modelCarMarker;
    [SerializeField] public float minimumZ;
    [SerializeField] public float maximumZ;
    [SerializeField] public int numberOfParkingSlots;
    // Start is called before the first frame update
    void Start()
    {
        RaceSettingsSingleton.Instance.sequentialParking = m_sequentialParking.gameObject;
        RaceSettingsSingleton.Instance.overheadCamera = m_overheadCamera;
        RaceSettingsSingleton.Instance.behindCarCamera = m_behindCarCamera;
        RaceSettingsSingleton.Instance.playerParking = m_playerParking.gameObject;
        RaceSettingsSingleton.Instance.playerOverheadCamera = m_playerOverheadCamera;
        RaceSettingsSingleton.Instance.playerBehindCarCamera = m_playerBehindCarCamera;
        RaceSettingsSingleton.Instance.modelParking = m_modelParking.gameObject;
        RaceSettingsSingleton.Instance.modelOverheadCamera = m_modelOverheadCamera;
        RaceSettingsSingleton.Instance.modelBehindCarCamera = m_modelBehindCarCamera;

        RaceSettingsSingleton.Instance.playerOverheadCamera.rect = new Rect(0, 0, 0.5f, 1);
        RaceSettingsSingleton.Instance.playerBehindCarCamera.rect = new Rect(0, 0, 0.5f, 1);
        RaceSettingsSingleton.Instance.modelOverheadCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
        RaceSettingsSingleton.Instance.modelBehindCarCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
        RaceSettingsSingleton.Instance.sequentialStartMarker = m_sequentialCarMarker;
        RaceSettingsSingleton.Instance.playerStartMarker = m_playerCarMarker;
        RaceSettingsSingleton.Instance.modelStartMarker = m_modelCarMarker;
        RaceSettingsSingleton.Instance.minimumZ = minimumZ;
        RaceSettingsSingleton.Instance.maximumZ = maximumZ;
        RaceSettingsSingleton.Instance.updateSetStartSlider();

        RaceSettingsSingleton.Instance.numberOfParkingSlots = numberOfParkingSlots;
        RaceSettingsSingleton.Instance.updateSetEndSlider();

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
