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
    public bool setEnd = false;
    public GameObject overheadCamera;
    public GameObject behindCarCamera;
    public Transform startMarker;
    public float minimumZ;
    public float maximumZ;
    public float setStartZ;
    public Slider setStartSlider;
    public int numberOfParkingSlots;
    public Slider setEndSlider;
    public List<ParkingSlot> parkingSlots;
    private bool setEndSliderPressed;
    private int setEndParkingSlotNumber;

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
    public int calculateTargetSlot()
    {
        if (setEnd) return setEndParkingSlotNumber;
        return Random.Range(0, numberOfParkingSlots);
    }
    public void updateSetStartSlider()
    {
        setStartSlider.minValue = minimumZ;
        setStartSlider.maxValue = maximumZ;
        setStartSlider.value = 0;
    }
    public void updateSetEndSlider()
    {
        setEndSlider.minValue = 0;
        setEndSlider.maxValue = numberOfParkingSlots - 1;
        setEndSlider.value = 0;
    }
    public void startSetEnd(int currentValue)
    {
        setEndSliderPressed = true;
        setEndParkingSlotNumber = currentValue;
        parkingSlots[currentValue].MarkSetEnd();
    }
    public void endSetEnd(int currentValue)
    {
        setEndSliderPressed = false;
        setEndParkingSlotNumber = currentValue;
        parkingSlots[currentValue].UnmarkSetEnd();
    }
    public void updateSetEnd(int currentValue)
    {
        parkingSlots[setEndParkingSlotNumber].UnmarkSetEnd();
        parkingSlots[currentValue].MarkSetEnd();
        setEndParkingSlotNumber = currentValue;
    }
    public void setDefault()
    {
        autoRestart = true;
        otherCars = true;
        manualRestart = false;
        setStart = false;
        setStartZ = 0;
        setEnd = false;
        setEndParkingSlotNumber = 0;
    }
}
