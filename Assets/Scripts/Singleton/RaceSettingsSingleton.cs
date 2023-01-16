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

    public GameObject sequentialParking;
    public GameObject overheadCamera;
    public GameObject behindCarCamera;
    public Transform sequentialStartMarker;
    public GameObject playerParking;
    public GameObject playerOverheadCamera;
    public GameObject playerBehindCarCamera;
    public Transform playerStartMarker;
    public GameObject modelParking;
    public GameObject modelOverheadCamera;
    public GameObject modelBehindCarCamera;
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
        targetParkingSlot = Random.Range(0, numberOfParkingSlots);
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
    public void updateSetStartSlider()
    {
        setStartSlider.minValue = minimumZ;
        setStartSlider.maxValue = maximumZ;
        setStartSlider.value = 0;
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
