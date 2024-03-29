using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SimulationTest : MonoBehaviour
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    string modelName = "DobryModel";
    string sceneName = "Parallel";
    string overlayName = "SimulationOverlay";
    [UnityTest]
    public IEnumerator CheckSimulationRestartTest()
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
        var script = GameObject.Find("car-root").GetComponent<SimulationCarAgent>();
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
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var toggle = GameObject.Find("OtherCars").GetComponent<Toggle>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        Assert.AreEqual(true, toggle.isOn); // This toggle should be 'on' by default
        int activeCars = countActiveParkedCars(); // Count the number of static cars
        Assert.That(activeCars, Is.GreaterThan(0)); // There should be at least one static car
        toggle.isOn = false; // Turn off the static cars
        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart

        // Assert
        activeCars = countActiveParkedCars(); // Count the number of static cars after restart
        Assert.AreEqual(0, activeCars); // There should be no more static cars
    }
    [UnityTest]
    public IEnumerator CheckCameraTest()
    {
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var dropdown = GameObject.Find("Options").GetComponent<TMP_Dropdown>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var overheadCamera = GameObject.Find("OverheadCamera");
        var behindCarCamera = GameObject.Find("BehindCarCamera");
        Assert.AreEqual(0, dropdown.value); // Default option is "Lot ptaka", index 0
        Assert.IsNotNull(overheadCamera); // It should be active
        Assert.IsNull(behindCarCamera); // It shouldn't be active, so it shouldn't be found, so it should be null

        dropdown.value = 1; // Change to "Zza samochodu", index 1
        yield return null; // Let the camera change take place
        overheadCamera = GameObject.Find("OverheadCamera");
        behindCarCamera = GameObject.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value); // Option changed
        Assert.IsNull(overheadCamera); // It shouldn't be active, so it shouldn't be found, so it should be null
        Assert.IsNotNull(behindCarCamera); // It should be active

        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart
        overheadCamera = GameObject.Find("OverheadCamera"); // Check if the changes remained after simulation restart
        behindCarCamera = GameObject.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value); // Option changed
        Assert.IsNull(overheadCamera); // It shouldn't be active, so it shouldn't be found, so it should be null
        Assert.IsNotNull(behindCarCamera); // It should be active
    }
    [UnityTest]
    public IEnumerator CheckAutoRestartTest()
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
        var toggle = GameObject.Find("Restart").GetComponent<Toggle>();
        var script = GameObject.Find("car-root").GetComponent<SimulationCarAgent>();
        var parking = GameObject.Find("parallelParkingArea"); // We check whether it still exists - if it does, that means the summary was not loaded
        Assert.AreEqual(true, toggle.isOn); // This toggle should be 'on' by default
        Assert.AreEqual(1, script.NumberOfSimulations); // This is first simulation
        Assert.IsNotNull(parking); // Parking exists

        script.EndEpisode(); // We "end" the simulation, not by manual restart
        yield return null; // Let the simulation restart

        parking = GameObject.Find("parallelParkingArea");
        Assert.AreEqual(true, toggle.isOn); // Is still on
        Assert.AreEqual(2, script.NumberOfSimulations); // This is second simulation
        Assert.IsNotNull(parking); // Parking exists
        toggle.isOn = false; // We turn off autoRestart
        script.EndEpisode(); // We "end" the simulation, not by manual restart
        yield return null; // Let the simulation restart

        parking = GameObject.Find("parallelParkingArea");
        Assert.IsNull(parking); // Parking should disappear
        var closeSummaryButton = GameObject.Find("Button").GetComponent<Button>();
        Assert.IsNotNull(closeSummaryButton); // Summary should appear
        closeSummaryButton.onClick.Invoke();
        yield return null; // Let the parking load
        yield return null; // Let the simulation start

        var checkCloseSummary = GameObject.Find("Button");
        parking = GameObject.Find("parallelParkingArea"); // Parking should appear again
        Assert.IsNull(checkCloseSummary); // This toggle should be 'on' by default
        Assert.IsNotNull(parking); // Parking exists
    }
    [UnityTest]
    public IEnumerator CheckSetStart()
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
        var script = GameObject.Find("car-root").GetComponent<SimulationCarAgent>();
        var car = GameObject.Find("car-root");
        Assert.AreEqual(false, toggle.isOn); // This toggle should be 'off' by default
        Assert.AreEqual(1, script.NumberOfSimulations); // This is first simulation
        Assert.AreEqual(0, slider.value); // Default value of slider is 0
        Assert.AreEqual(-20, slider.minValue); // This map has minimum value of -20
        Assert.AreEqual(20, slider.maxValue); // This map has maximum value of 20

        toggle.isOn = true; // Turn on setStart position
        yield return null; // Let the slider turn on
        slider.value = carStart; // Set the new start position

        for(int i = 0; i < 3; i++) // Make sure that the option works. Check it 3 times to make sure that it was not an accident
        {
            button.onClick.Invoke(); // Restart the simulation
            yield return null; // Let the simulation restart
            Assert.IsTrue(car.transform.localPosition.z < carStart + epsilon); // Check whether the car is close enough to the requested start position
            Assert.IsTrue(car.transform.localPosition.z > carStart - epsilon); // Check whether the car is close enough to the requested start position
        }
    }
    private int countActiveParkedCars()
    {
        string parkingSlotName = "parkingSlot";
        string staticCarName = "static-car";
        int i = 1;
        int carCount = 0;
        var gameObject = GameObject.Find(parkingSlotName);
        if (gameObject == null) return 0;
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
