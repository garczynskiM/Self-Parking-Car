using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSimulationMap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform m_autoRestartTransform;
    [SerializeField] private Transform m_otherCarsTransform;
    void Start()
    {
        if(!StaticVar.loadOnlyOnce)
        {
            StaticVar.m_autoRestartTransform = m_autoRestartTransform;
            StaticVar.m_otherCarsTransform = m_otherCarsTransform;
            SceneManager.LoadScene(StaticVar.sceneName, LoadSceneMode.Additive);
            StaticVar.loadOnlyOnce = true;
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Loaded UI for simulation");
    }
}
