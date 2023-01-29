using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadRaceMap : MonoBehaviour
{
    [SerializeField] private Transform m_otherCarsTransform;
    [SerializeField] private Slider setStartSlider;
    [SerializeField] private Slider setEndSlider;
    void Start()
    {
        if (!MapLoadVarsSingleton.Instance.loadOnlyOnce)
        {
            RaceSettingsSingleton.Instance.setDefault();
            RaceSettingsSingleton.Instance.setStartSlider = setStartSlider;
            RaceSettingsSingleton.Instance.setEndSlider = setEndSlider;
            RaceSettingsSingleton.Instance.playerManualRestart = false;
            RaceSettingsSingleton.Instance.carManualRestart = false;
            RaceSettingsSingleton.Instance.manualRestart = false;
            MapLoadVarsSingleton.Instance.m_otherCarsTransform = m_otherCarsTransform;
            m_otherCarsTransform.GetComponentInChildren<Toggle>().isOn = RaceSettingsSingleton.Instance.otherCars;
            SceneManager.LoadScene(MapLoadVarsSingleton.Instance.sceneName, LoadSceneMode.Additive);
            MapLoadVarsSingleton.Instance.loadOnlyOnce = true;
        }
    }
}
