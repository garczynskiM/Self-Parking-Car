using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationSettingsSingleton : MonoBehaviour
{
    public bool autoRestart = true;
    public bool otherCars = true;
    public bool manualRestart = false;
    public GameObject overheadCamera;
    public GameObject behindCarCamera;

    public static SimulationSettingsSingleton Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
