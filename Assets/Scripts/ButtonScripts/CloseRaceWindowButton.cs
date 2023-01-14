using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloseRaceWindowButton : MonoBehaviour
{
    public void CloseRaceSummary()
    {
        RaceSummarySingleton.Instance.summaryClosed = true;
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        SceneManager.LoadScene("RaceOverlay", LoadSceneMode.Single);
    }
}
