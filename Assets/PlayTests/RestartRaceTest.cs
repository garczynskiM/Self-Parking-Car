using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class RestartRaceTest : MonoBehaviour
{
    [UnityTest]
    public IEnumerator CheckRaceSequentialRestartTest()
    {
        // Arrange
        MapLoadStaticVars.sceneName = "RaceParallel";
        MapLoadStaticVars.modelInfo = new ModelInfo("CarBehavior 2");
        MapLoadStaticVars.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync("RaceOverlay"); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var script = GameObject.Find("car-root").GetComponent<SequentialRaceCarAgent>();
        Assert.AreEqual(1, script.NumberOfSimulations); // This should be the first simulation
        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart

        // Assert
        Assert.AreEqual(2, script.NumberOfSimulations); // This should be the second simulation
    }
    [UnityTest]
    public IEnumerator CheckObstacleSpawnTest()
    {
        // Arrange
        MapLoadStaticVars.sceneName = "RaceParallel";
        MapLoadStaticVars.modelInfo = new ModelInfo("CarBehavior 2");
        MapLoadStaticVars.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync("RaceOverlay"); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var toggle = GameObject.Find("OtherCars").GetComponent<Toggle>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        Assert.AreEqual(true, toggle.isOn); // This toggle should be 'on' by default
        int activeCars = countSequentialActiveParkedCars(); // Count the number of static cars
        Assert.That(activeCars, Is.GreaterThan(0)); // There should be at least one static car
        toggle.isOn = false; // Turn off the static cars
        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart

        // Assert
        activeCars = countSequentialActiveParkedCars(); // Count the number of static cars after restart
        Assert.AreEqual(0, activeCars); // There should be no more static cars
    }
    [UnityTest]
    public IEnumerator CheckCameraTest()
    {
        MapLoadStaticVars.sceneName = "RaceParallel";
        MapLoadStaticVars.modelInfo = new ModelInfo("CarBehavior 2");
        MapLoadStaticVars.loadOnlyOnce = false;
        string sequentialParkingName = "sequentialParking";

        yield return SceneManager.LoadSceneAsync("RaceOverlay"); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var cameraSettings = GameObject.Find("CameraSettings");
        var dropdown = cameraSettings.transform.Find("Options").GetComponent<TMP_Dropdown>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();

        var sequentialParking = GameObject.Find(sequentialParkingName);
        var overheadCamera = sequentialParking.transform.Find("OverheadCamera");
        var car = sequentialParking.transform.Find("car-root");
        var behindCarCamera = car.Find("BehindCarCamera");
        Assert.AreEqual(0, dropdown.value); // Default option is "Lot ptaka", index 0
        Assert.IsTrue(overheadCamera.gameObject.activeSelf); // It should be active
        Assert.IsFalse(behindCarCamera.gameObject.activeSelf); // It shouldn't be active, so it shouldn't be found, so it should be null

        dropdown.value = 1; // Change to "Zza samochodu", index 1
        yield return null; // Let the camera change take place
        sequentialParking = GameObject.Find(sequentialParkingName);
        overheadCamera = sequentialParking.transform.Find("OverheadCamera");
        car = sequentialParking.transform.Find("car-root");
        behindCarCamera = car.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value); // Option changed
        Assert.IsFalse(overheadCamera.gameObject.activeSelf); // It shouldn't be active, so it shouldn't be found, so it should be null
        Assert.IsTrue(behindCarCamera.gameObject.activeSelf); // It should be active

        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart
        sequentialParking = GameObject.Find(sequentialParkingName);
        overheadCamera = sequentialParking.transform.Find("OverheadCamera");
        car = sequentialParking.transform.Find("car-root");
        behindCarCamera = car.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value); // Option changed
        Assert.IsFalse(overheadCamera.gameObject.activeSelf); // It shouldn't be active, so it shouldn't be found, so it should be null
        Assert.IsTrue(behindCarCamera.gameObject.activeSelf); // It should be active
    }
    private int countSequentialActiveParkedCars()
    {
        string parkingSlotName = "parkingSlot";
        string sequentialParkingName = "sequentialParking";
        string parkingAreaName = "parkingArea";
        string staticCarName = "static-car";
        int i = 1;
        int carCount = 0;
        var sequentialParking = GameObject.Find(sequentialParkingName);
        if (sequentialParking == null || !sequentialParking.activeSelf) return 0;
        var parkingArea = sequentialParking.transform.Find(parkingAreaName);
        if (parkingArea == null || !sequentialParking.activeSelf) return 0;
        var gameObject = parkingArea.Find(parkingSlotName).gameObject;
        if (gameObject == null || !sequentialParking.activeSelf) return 0;
        var staticCar = gameObject.transform.Find(staticCarName);
        while (staticCar != null)
        {
            if (staticCar.gameObject.activeSelf) carCount++;
            gameObject = GameObject.Find(string.Format("{0} ({1})", parkingSlotName, i));
            if (gameObject == null) break;
            staticCar = gameObject.transform.Find(staticCarName);
            i++;
        }
        return carCount;
    }
}
