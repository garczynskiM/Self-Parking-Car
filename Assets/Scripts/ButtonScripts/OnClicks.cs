using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClicks : MonoBehaviour
{
    [SerializeField] private Transform m_MapContentContainer;
    public void startSimulationMode()
    {
        string title = ChooseMap();
        MapLoadStaticVars.sceneName = title;
        //StaticVar.sceneName = "smallEmptyParking";
        MapLoadStaticVars.loadOnlyOnce = false;
        SceneManager.LoadScene("SimulationOverlay", LoadSceneMode.Single);
    }
    private string ChooseMap()
    {
        int childCount = m_MapContentContainer.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var mapItems = m_MapContentContainer.GetChild(i);
            var toggle = mapItems.GetComponentInChildren<Toggle>();
            if (toggle.isOn)
            {
                var title = mapItems.GetChild(1);
                string mapName = title.GetComponent<Text>().text;
                return mapName;
            }
        }
        return null;
    }
}
