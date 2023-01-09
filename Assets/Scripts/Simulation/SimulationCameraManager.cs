using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SimulationCameraMode
{
    Overhead,
    BehindCar,
}
public class SimulationCameraManager : MonoBehaviour
{
    //GameObject overheadCamera;
    //GameObject behindCarCamera;
    //bool camerasInit;
    // Start is called before the first frame update
    /*void Start()
    {
        camerasInit = false;
    }
    void InitCameras()
    {
        overheadCamera = GameObject.Find("OverheadCamera");
        behindCarCamera = GameObject.Find("BehindCarCamera");
    }*/
    public void DropdownChangeValue(TMP_Dropdown change)
    {
        //if (!camerasInit) InitCameras();
        SimulationCameraMode cameraMode = (SimulationCameraMode)change.value;
        switch(cameraMode)
        {
            case SimulationCameraMode.Overhead:
                SimulationSettingsSingleton.Instance.overheadCamera.SetActive(true);
                SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(false);
                break;
            case SimulationCameraMode.BehindCar:
                SimulationSettingsSingleton.Instance.overheadCamera.SetActive(false);
                SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(true);
                break;
            default:
                break;
        }
    }
}
