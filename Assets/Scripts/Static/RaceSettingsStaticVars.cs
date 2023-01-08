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
public class RaceSettingsStaticVars : MonoBehaviour
{
    public static bool otherCars = true;
    public static RaceOrder raceOrder = RaceOrder.FirstPlayerThenModel;
    public static RaceCameraMode raceCameraMode = RaceCameraMode.Overhead;
    public static bool manualRestart = false;

    public static GameObject sequentialParking;
    public static GameObject overheadCamera;
    public static GameObject behindCarCamera;
    public static GameObject playerParking;
    public static GameObject playerOverheadCamera;
    public static GameObject playerBehindCarCamera;
    public static GameObject modelParking;
    public static GameObject modelOverheadCamera;
    public static GameObject modelBehindCarCamera;

    public static bool parkingGenerated;
    public static int numberOfParkingSlots;
    public static int targetParkingSlot;
    public static List<int> occupiedParkingSlots;
    [SerializeField] public static float minCarRespawnZ;
    [SerializeField] public static float maxCarRespawnZ;
    public static float currentCarRespawnZ;

    public static bool carManualRestart = false;
    public static bool playerManualRestart = false;

    public static void generateParkingSlots()
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
