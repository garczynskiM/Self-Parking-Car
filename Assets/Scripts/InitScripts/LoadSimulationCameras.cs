using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadSimulationCameras : MonoBehaviour
{
    [SerializeField] public Transform carMarker;
    [SerializeField] public float minimumZ;
    [SerializeField] public float maximumZ;
    [SerializeField] public int numberOfParkingSlots;
    void Start()
    {
        SimulationSettingsSingleton.Instance.overheadCamera = GameObject.Find("OverheadCamera");
        SimulationSettingsSingleton.Instance.behindCarCamera = GameObject.Find("BehindCarCamera");
        SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(false);
        SimulationSettingsSingleton.Instance.startMarker = carMarker;
        SimulationSettingsSingleton.Instance.minimumZ = minimumZ;
        SimulationSettingsSingleton.Instance.maximumZ = maximumZ;
        SimulationSettingsSingleton.Instance.updateSetStartSlider();
        SimulationSettingsSingleton.Instance.numberOfParkingSlots = numberOfParkingSlots;
        SimulationSettingsSingleton.Instance.updateSetEndSlider();
        carMarker.gameObject.SetActive(false);
    }
}
