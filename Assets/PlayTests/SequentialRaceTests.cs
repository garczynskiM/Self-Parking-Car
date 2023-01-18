using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SequentialRaceTest : MonoBehaviour
{
    string modelName = "DobryModel";
    string sceneName = "RaceParallel";
    string overlayName = "RaceOverlay";
    [UnityTest]
    public IEnumerator CheckRaceSequentialRestartTest()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
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
    public IEnumerator CheckRaceSequentialObstacleSpawnTest()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start <----
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
    public IEnumerator CheckRaceSequentialCameraTest()
    {
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        string sequentialParkingName = "sequentialParking";

        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
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
    [UnityTest]
    public IEnumerator CheckRaceSequentialSameParkingTest()
    {
        // Arrange
        float epsilon = 0.01f;
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var script = GameObject.Find("car-root").GetComponent<SequentialRaceCarAgent>();
        var carZ = script.CurrentTransformZ;
        var occupiedParkingSlots = script.ListOfOccupiedSpaces;
        var currentSlot = script.CurrentSlotNumber;
        script.EndEpisode();
        yield return null; // Let the simulation restart

        // Assert
        var newCarZ = script.CurrentTransformZ;
        var newOccupiedParkingSlots = script.ListOfOccupiedSpaces;
        var newCurrentSlot = script.CurrentSlotNumber;
        Assert.IsTrue(newCarZ < carZ + epsilon);
        Assert.IsTrue(newCarZ > carZ - epsilon);
        Assert.AreEqual(occupiedParkingSlots, newOccupiedParkingSlots);
        Assert.AreEqual(currentSlot, newCurrentSlot);
    }
    [UnityTest]
    public IEnumerator CheckRaceSequentialProcedureTest()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var script = GameObject.Find("car-root").GetComponent<SequentialRaceCarAgent>();
        var parking = GameObject.Find("sequentialParking");
        Assert.IsNotNull(parking); // player is racing, it should not be null
        script.EndEpisode();
        yield return null; // Let the race advance

        parking = GameObject.Find("sequentialParking");
        Assert.IsNotNull(parking); // model is racing, it should not be null
        script.EndEpisode();
        yield return null; // Let the race advance

        parking = GameObject.Find("sequentialParking");
        Assert.IsNull(parking); // Parking should disappear
        var closeSummaryButton = GameObject.Find("Button").GetComponent<Button>();
        Assert.IsNotNull(closeSummaryButton); // Summary should appear
        closeSummaryButton.onClick.Invoke();
        yield return null; // Let the parking load
        yield return null; // Let the simulation start

        var checkCloseSummary = GameObject.Find("Button");
        parking = GameObject.Find("sequentialParking"); 
        Assert.IsNull(checkCloseSummary); // Summary is closed
        Assert.IsNotNull(parking); // Parking exists, so race was loaded
    }
    [UnityTest]
    public IEnumerator CheckRaceSequentialSetStart()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        float epsilon = 0.01f;
        float carStart = -8f;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var gameObject = GameObject.Find("StartPosition");
        var toggle = gameObject.GetComponentInChildren<Toggle>();
        var slider = gameObject.GetComponentInChildren<Slider>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var script = GameObject.Find("car-root").GetComponent<SequentialRaceCarAgent>();
        var car = GameObject.Find("car-root");
        Assert.AreEqual(false, toggle.isOn); // This toggle should be 'off' by default
        Assert.AreEqual(1, script.NumberOfSimulations); // This is first simulation
        Assert.AreEqual(0, slider.value); // Default value of slider is 0
        Assert.AreEqual(-20, slider.minValue); // This map has minimum value of -20
        Assert.AreEqual(20, slider.maxValue); // This map has maximum value of 20

        toggle.isOn = true; // Turn on setStart position
        yield return null; // Let the slider turn on
        slider.value = carStart; // Set the new start position

        for (int i = 0; i < 3; i++) // Make sure that the option works. Check it 3 times to make sure that it was not an accident
        {
            button.onClick.Invoke(); // Restart the simulation
            yield return null; // Let the simulation restart
            Assert.IsTrue(car.transform.localPosition.z < carStart + epsilon); // Check whether the car is close enough to the requested start position
            Assert.IsTrue(car.transform.localPosition.z > carStart - epsilon); // Check whether the car is close enough to the requested start position
        }
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
