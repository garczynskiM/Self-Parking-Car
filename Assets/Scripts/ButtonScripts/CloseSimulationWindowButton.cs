using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CloseSimulationWindowButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SimulationSummaryStaticVars.summaryClosed = true;
        SceneManager.UnloadSceneAsync("SimulationSummary");
    }
}
