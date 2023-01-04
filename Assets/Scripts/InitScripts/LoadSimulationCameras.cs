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
        SimulationSettingsStaticVars.overheadCamera = GameObject.Find("OverheadCamera");
        SimulationSettingsStaticVars.behindCarCamera = GameObject.Find("BehindCarCamera");
        SimulationSettingsStaticVars.behindCarCamera.SetActive(false);
    }
}
