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
        if (!MapLoadVarsSingleton.Instance.loadOnlyOnce)
        {
            RaceSettingsSingleton.Instance.playerManualRestart = false;
            RaceSettingsSingleton.Instance.carManualRestart = false;
            RaceSettingsSingleton.Instance.manualRestart = false;
            //MapLoadVarsSingleton.m_autoRestartTransform = m_autoRestartTransform;
            //m_autoRestartTransform.GetComponentInChildren<Toggle>().isOn = SimulationSettingsSingleton.autoRestart;
            MapLoadVarsSingleton.Instance.m_otherCarsTransform = m_otherCarsTransform;
            m_otherCarsTransform.GetComponentInChildren<Toggle>().isOn = RaceSettingsSingleton.Instance.otherCars;
            SceneManager.LoadScene(MapLoadVarsSingleton.Instance.sceneName, LoadSceneMode.Additive);
            MapLoadVarsSingleton.Instance.loadOnlyOnce = true;
        }
    }
}
