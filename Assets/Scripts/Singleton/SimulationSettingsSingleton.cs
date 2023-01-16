using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationSettingsSingleton : MonoBehaviour
{
    public bool autoRestart = true;
    public bool otherCars = true;
    public bool manualRestart = false;
    public bool setStart = false;
    public GameObject overheadCamera;
    public GameObject behindCarCamera;
    public Transform startMarker;
    public float minimumZ;
    public float maximumZ;
    public float setStartZ;
    public Slider setStartSlider;

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
    public float calculateZCarSpawn()
    {
        if (setStart) return setStartZ;
        else return Random.Range(minimumZ, maximumZ);
    }
    public void updateSetStartSlider()
    {
        setStartSlider.minValue = minimumZ;
        setStartSlider.maxValue = maximumZ;
        setStartSlider.value = 0;
    }
    public void setDefault()
    {
        autoRestart = true;
        otherCars = true;
        manualRestart = false;
        setStart = false;
        setStartZ = 0;
    }
}
