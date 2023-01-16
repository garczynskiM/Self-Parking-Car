using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSimulationMap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform m_autoRestartTransform;
    [SerializeField] private Transform m_otherCarsTransform;
    [SerializeField] private Slider setStartSlider;
    void Start()
    {
        if(!MapLoadVarsSingleton.Instance.loadOnlyOnce)
        {
            SimulationSettingsSingleton.Instance.setDefault();
            SimulationSettingsSingleton.Instance.setStartSlider = setStartSlider;
            MapLoadVarsSingleton.Instance.m_autoRestartTransform = m_autoRestartTransform;
            m_autoRestartTransform.GetComponentInChildren<Toggle>().isOn = SimulationSettingsSingleton.Instance.autoRestart;
            MapLoadVarsSingleton.Instance.m_otherCarsTransform = m_otherCarsTransform;
            m_otherCarsTransform.GetComponentInChildren<Toggle>().isOn = SimulationSettingsSingleton.Instance.otherCars;
            SceneManager.LoadScene(MapLoadVarsSingleton.Instance.sceneName, LoadSceneMode.Additive);
            MapLoadVarsSingleton.Instance.loadOnlyOnce = true;
        }
    }
}
