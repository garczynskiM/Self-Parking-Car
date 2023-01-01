using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadRaceMap : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private Transform m_autoRestartTransform;
    [SerializeField] private Transform m_otherCarsTransform;
    void Start()
    {
        if (!MapLoadStaticVars.loadOnlyOnce)
        {
            //MapLoadStaticVars.m_autoRestartTransform = m_autoRestartTransform;
            //m_autoRestartTransform.GetComponentInChildren<Toggle>().isOn = SimulationSettingsStaticVars.autoRestart;
            MapLoadStaticVars.m_otherCarsTransform = m_otherCarsTransform;
            m_otherCarsTransform.GetComponentInChildren<Toggle>().isOn = SimulationSettingsStaticVars.otherCars;
            SceneManager.LoadScene(MapLoadStaticVars.sceneName, LoadSceneMode.Additive);
            MapLoadStaticVars.loadOnlyOnce = true;
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Loaded UI for race");
    }
}
