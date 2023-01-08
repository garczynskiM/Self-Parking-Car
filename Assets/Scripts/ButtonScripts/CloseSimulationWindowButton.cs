using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CloseSimulationWindowButton : MonoBehaviour
{
    public void OnClick()
    {
        SimulationSummaryStaticVars.summaryClosed = true;
        MapLoadStaticVars.loadOnlyOnce = false;
        SceneManager.LoadScene("SimulationOverlay", LoadSceneMode.Single);
    }
}
