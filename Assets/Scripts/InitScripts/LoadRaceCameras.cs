using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadRaceCameras : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RaceSettingsStaticVars.overheadCamera = GameObject.Find("OverheadCamera");
        RaceSettingsStaticVars.behindCarCamera = GameObject.Find("BehindCarCamera");
        RaceSettingsStaticVars.behindCarCamera.SetActive(false);
        //TODO: Add cameras for parallel race
    }
}
