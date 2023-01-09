using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public GameObject sequentialParking;
    public GameObject overheadCamera;
    public GameObject behindCarCamera;
    public GameObject playerParking;
    public GameObject playerOverheadCamera;
    public GameObject playerBehindCarCamera;
    public GameObject modelParking;
    public GameObject modelOverheadCamera;
    public GameObject modelBehindCarCamera;

    public bool parkingGenerated;
    public int numberOfParkingSlots;
    public int targetParkingSlot;
    public List<int> occupiedParkingSlots;
    [SerializeField] public float minCarRespawnZ;
    [SerializeField] public float maxCarRespawnZ;
    public float currentCarRespawnZ;

    public bool carManualRestart = false;
    public bool playerManualRestart = false;

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
        currentCarRespawnZ = Random.Range(minCarRespawnZ, maxCarRespawnZ);

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
}
