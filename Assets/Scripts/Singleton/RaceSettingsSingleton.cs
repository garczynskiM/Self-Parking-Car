using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum RaceOrder
{
    FirstPlayerThenModel,
    PlayerAndModel,
    None
}
public class RaceSettingsSingleton : MonoBehaviour
{
    public bool otherCars = true;
    public RaceOrder raceOrder = RaceOrder.FirstPlayerThenModel;
    public RaceCameraMode raceCameraMode = RaceCameraMode.Overhead;
    public bool manualRestart = false;
    public bool setStart = false;
    public bool setEnd = false;

    public GameObject sequentialParking;
    public Camera overheadCamera;
    public Camera behindCarCamera;
    public Transform sequentialStartMarker;
    public GameObject playerParking;
    public Camera playerOverheadCamera;
    public Camera playerBehindCarCamera;
    public Transform playerStartMarker;
    public GameObject modelParking;
    public Camera modelOverheadCamera;
    public Camera modelBehindCarCamera;
    public Transform modelStartMarker;

    public bool parkingGenerated;
    public int numberOfParkingSlots;
    public int targetParkingSlot;
    public List<int> occupiedParkingSlots;
    public float minimumZ;
    public float maximumZ;
    public float setStartZ;
    public float currentCarRespawnZ;

    public bool carManualRestart = false;
    public bool playerManualRestart = false;
    public Slider setStartSlider;

    public Slider setEndSlider;

    public List<ParkingSlot> sequentialParkingSlots;
    public List<ParkingSlot> playerParkingSlots;
    public List<ParkingSlot> modelParkingSlots;
    private bool setEndSliderPressed;
    private int setEndParkingSlotNumber;
    public static RaceSettingsSingleton Instance;
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

    public void generateParkingSlots()
    {
        if (parkingGenerated) return;
        targetParkingSlot = calculateTargetSlot();
        occupiedParkingSlots = new List<int>();
        currentCarRespawnZ = calculateZCarSpawn();
        if (otherCars)
        {
            var tempParkingSlots = Enumerable.Range(0, numberOfParkingSlots).ToList();
            tempParkingSlots.RemoveAt(targetParkingSlot);
            int occupySize = Random.Range(tempParkingSlots.Count / 2, tempParkingSlots.Count);
            while (occupySize > 0)
            {
                int tempOccupied = Random.Range(0, tempParkingSlots.Count);
                occupiedParkingSlots.Add(tempParkingSlots[tempOccupied]);
                tempParkingSlots.RemoveAt(tempOccupied);
                occupySize--;
            }
        }
        parkingGenerated = true;
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
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                sequentialParkingSlots[currentValue].MarkSetEnd();
                break;
            case RaceOrder.PlayerAndModel:
                playerParkingSlots[currentValue].MarkSetEnd();
                modelParkingSlots[currentValue].MarkSetEnd();
                break;
            default:
                break;
        }
    }
    public void endSetEnd(int currentValue)
    {
        setEndSliderPressed = false;
        setEndParkingSlotNumber = currentValue;
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                sequentialParkingSlots[currentValue].UnmarkSetEnd();
                break;
            case RaceOrder.PlayerAndModel:
                playerParkingSlots[currentValue].UnmarkSetEnd();
                modelParkingSlots[currentValue].UnmarkSetEnd();
                break;
            default:
                break;
        }
    }
    public void updateSetEnd(int currentValue)
    {
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                sequentialParkingSlots[setEndParkingSlotNumber].UnmarkSetEnd();
                sequentialParkingSlots[currentValue].MarkSetEnd();
                break;
            case RaceOrder.PlayerAndModel:
                playerParkingSlots[setEndParkingSlotNumber].UnmarkSetEnd();
                modelParkingSlots[setEndParkingSlotNumber].UnmarkSetEnd();
                playerParkingSlots[currentValue].MarkSetEnd();
                modelParkingSlots[currentValue].MarkSetEnd();
                break;
            default:
                break;
        }
        setEndParkingSlotNumber = currentValue;
    }
    public void setDefault()
    {
        otherCars = true;
        manualRestart = false;
        setStart = false;
        setStartZ = 0;
        raceOrder = RaceOrder.FirstPlayerThenModel;
        raceCameraMode = RaceCameraMode.Overhead;
    }
    public void activateStartMarker(bool active)
    {
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                sequentialStartMarker.gameObject.SetActive(active);
                break;
            case RaceOrder.PlayerAndModel:
                playerStartMarker.gameObject.SetActive(active);
                modelStartMarker.gameObject.SetActive(active);
                break;
            default:
                break;
        }
    }
    public void setStartMarkerPosition(float zValue)
    {
        switch (raceOrder)
        {
            case RaceOrder.FirstPlayerThenModel:
                float currentSequentialZ = sequentialStartMarker.localPosition.z;
                sequentialStartMarker.localPosition = sequentialStartMarker.localPosition + new Vector3(0, 0, zValue - currentSequentialZ);
                break;
            case RaceOrder.PlayerAndModel:
                float currentPlayerZ = playerStartMarker.localPosition.z;
                float currentModelZ = modelStartMarker.localPosition.z;
                playerStartMarker.localPosition = playerStartMarker.localPosition + new Vector3(0, 0, zValue - currentPlayerZ);
                modelStartMarker.localPosition = modelStartMarker.localPosition + new Vector3(0, 0, zValue - currentModelZ);
                break;
            default:
                break;
        }
    }
}
