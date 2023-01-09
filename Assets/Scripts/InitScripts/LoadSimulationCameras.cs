using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadSimulationCameras : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SimulationSettingsSingleton.Instance.overheadCamera = GameObject.Find("OverheadCamera");
        SimulationSettingsSingleton.Instance.behindCarCamera = GameObject.Find("BehindCarCamera");
        SimulationSettingsSingleton.Instance.behindCarCamera.SetActive(false);
    }
}
