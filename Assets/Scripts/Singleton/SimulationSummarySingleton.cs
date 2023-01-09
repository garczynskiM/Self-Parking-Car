using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationSummarySingleton : MonoBehaviour
{
    public bool summaryClosed;
    public DateTime simulationStart;
    public DateTime simulationEnd;
    public bool parkingSuccessful;

    public static SimulationSummarySingleton Instance;
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
