using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSimulationMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!StaticVar.loadOnlyOnce)
        {
            SceneManager.LoadScene(StaticVar.sceneName, LoadSceneMode.Additive);
            StaticVar.loadOnlyOnce = true;
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Loaded UI for simulation");
    }
}
