using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModelInfo
{
    public string name;
    public ModelInfo(string name)
    {
        this.name = name;
    }
    // rzeczy potrzebne do dostosowania obserwacji itd.
}
public class OnClicks : MonoBehaviour
{
    [SerializeField] private Transform m_MapContentContainer;
    [SerializeField] private Transform m_ModelContentContainer;
    public void startSimulationMode()
    {
        string title = ChooseMap();
        ModelInfo modelInfo = ChooseModel();
        MapLoadStaticVars.sceneName = title;
        MapLoadStaticVars.modelInfo = modelInfo;
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
    private ModelInfo ChooseModel()
    {
        int childCount = m_ModelContentContainer.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var mapItems = m_ModelContentContainer.GetChild(i);
            var toggle = mapItems.GetComponentInChildren<Toggle>();
            if (toggle.isOn)
            {
                var title = mapItems.GetChild(0);
                string modelName = title.GetComponent<Text>().text;
                ModelInfo result = new ModelInfo(modelName);
                return result;
            }
        }
        return null;
    }
    public void startRaceMode()
    {
        string title = ChooseMap();
        ModelInfo modelInfo = ChooseModel();
        MapLoadStaticVars.sceneName = "Race" + title;
        MapLoadStaticVars.modelInfo = modelInfo;
        //StaticVar.sceneName = "smallEmptyParking";
        MapLoadStaticVars.loadOnlyOnce = false;
        SceneManager.LoadScene("RaceOverlay", LoadSceneMode.Single);
    }
}
