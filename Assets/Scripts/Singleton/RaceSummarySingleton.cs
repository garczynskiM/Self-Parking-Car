using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSummarySingleton : MonoBehaviour
{
    public bool summaryClosed;
    public DateTime playerStart;
    public DateTime playerEnd;
    public bool playerParkingSuccessful;
    public DateTime carStart;
    public DateTime carEnd;
    public bool carParkingSuccessful;

    public static RaceSummarySingleton Instance;
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
