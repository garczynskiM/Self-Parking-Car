using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ParallelRaceTest : MonoBehaviour
{
    string modelName = "DobryModel";
    string sceneName = "RaceParallel";
    string overlayName = "RaceOverlay";
    string sequentialParkingName = "sequentialParking";
    string playerParkingName = "playerParking";
    string modelParkingName = "modelParking";
    [UnityTest]
    public IEnumerator CheckRaceModeSwitchTest()
    {
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var sequentialParking = GameObject.Find(sequentialParkingName); // The parking for default mode of racing
        var playerParking = GameObject.Find(playerParkingName); // Parkings for parallel racing mode
        var modelParking = GameObject.Find(modelParkingName);
        Assert.IsNotNull(sequentialParking);
        Assert.IsNull(playerParking); // They arenn't supposed to be active yet
        Assert.IsNull(modelParking);
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;

        sequentialParking = GameObject.Find(sequentialParkingName);
        playerParking = GameObject.Find(playerParkingName);
        modelParking = GameObject.Find(modelParkingName);
        Assert.IsNull(sequentialParking); // It should be gone now
        Assert.IsNotNull(playerParking); // They should be loaded now
        Assert.IsNotNull(modelParking);
        raceModeDropdown.value = 0; // Sequential racing
        yield return null; // Let the parkings change;

        sequentialParking = GameObject.Find(sequentialParkingName);
        playerParking = GameObject.Find(playerParkingName);
        modelParking = GameObject.Find(modelParkingName);
        Assert.IsNotNull(sequentialParking); // It should be gone now
        Assert.IsNull(playerParking); // They should be loaded now
        Assert.IsNull(modelParking);
    }
    [UnityTest]
    public IEnumerator CheckRaceParallelRestartTest()
    {
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;

        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var playerScript = GameObject.Find("playerCar").GetComponent<PlayerRaceCarAgent>();
        var modelScript = GameObject.Find("modelCar").GetComponent<ModelRaceCarAgent>();
        Assert.AreEqual(1, playerScript.NumberOfSimulations); // This should be the first race
        Assert.AreEqual(1, modelScript.NumberOfSimulations);
        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart

        Assert.AreEqual(2, playerScript.NumberOfSimulations); // This should be the second race
        Assert.AreEqual(2, modelScript.NumberOfSimulations);
    }
    [UnityTest]
    public IEnumerator CheckRaceParallelObstacleSpawnTest()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;

        var toggle = GameObject.Find("OtherCars").GetComponent<Toggle>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        Assert.AreEqual(true, toggle.isOn); // This toggle should be 'on' by default
        int playerActiveCars = countParallelActiveParkedCars(playerParkingName); // Count the number of static cars
        int modelActiveCars = countParallelActiveParkedCars(modelParkingName);
        Assert.That(playerActiveCars, Is.GreaterThan(0)); // There should be at least one static car
        Assert.That(modelActiveCars, Is.GreaterThan(0));
        toggle.isOn = false; // Turn off the static cars
        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the simulation restart

        // Assert
        playerActiveCars = countParallelActiveParkedCars(playerParkingName); // Count the number of static cars after restart
        modelActiveCars = countParallelActiveParkedCars(modelParkingName);
        Assert.AreEqual(0, playerActiveCars); // There should be no more static cars
        Assert.AreEqual(0, modelActiveCars);
    }
    [UnityTest]
    public IEnumerator CheckRaceParallelCameraTest()
    {
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;

        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;
        var cameraSettings = GameObject.Find("CameraSettings");
        var dropdown = cameraSettings.transform.Find("Options").GetComponent<TMP_Dropdown>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();

        var playerParking = GameObject.Find(playerParkingName); // Parkings for parallel racing mode
        var modelParking = GameObject.Find(modelParkingName);
        var playerOverheadCamera = playerParking.transform.Find("OverheadCamera");
        var modelOverheadCamera = modelParking.transform.Find("OverheadCamera");
        var playerCar = playerParking.transform.Find("playerCar");
        var modelCar = modelParking.transform.Find("modelCar");
        var playerBehindCarCamera = playerCar.Find("BehindCarCamera");
        var modelBehindCarCamera = modelCar.Find("BehindCarCamera");
        Assert.AreEqual(0, dropdown.value); // Default option is "Lot ptaka", index 0
        Assert.IsTrue(playerOverheadCamera.gameObject.activeSelf); // They should be active
        Assert.IsTrue(modelOverheadCamera.gameObject.activeSelf);
        Assert.IsFalse(playerBehindCarCamera.gameObject.activeSelf); // They shouldn't be active, so they shouldn't be found, so they should be null
        Assert.IsFalse(modelBehindCarCamera.gameObject.activeSelf);

        dropdown.value = 1; // Change to "Zza samochodu", index 1
        yield return null; // Let the camera change take place
        playerParking = GameObject.Find(playerParkingName);
        modelParking = GameObject.Find(modelParkingName);
        playerOverheadCamera = playerParking.transform.Find("OverheadCamera");
        modelOverheadCamera = modelParking.transform.Find("OverheadCamera");
        playerCar = playerParking.transform.Find("playerCar");
        modelCar = modelParking.transform.Find("modelCar");
        playerBehindCarCamera = playerCar.Find("BehindCarCamera");
        modelBehindCarCamera = modelCar.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value); // Option changed
        Assert.IsFalse(playerOverheadCamera.gameObject.activeSelf); // They shouldn't be active, so they shouldn't be found, so they should be null
        Assert.IsFalse(modelOverheadCamera.gameObject.activeSelf);
        Assert.IsTrue(playerBehindCarCamera.gameObject.activeSelf); // They should be active
        Assert.IsTrue(modelBehindCarCamera.gameObject.activeSelf);

        button.onClick.Invoke(); // Restart the simulation
        yield return null; // Let the camera change take place
        playerParking = GameObject.Find(playerParkingName);
        modelParking = GameObject.Find(modelParkingName);
        playerOverheadCamera = playerParking.transform.Find("OverheadCamera");
        modelOverheadCamera = modelParking.transform.Find("OverheadCamera");
        playerCar = playerParking.transform.Find("playerCar");
        modelCar = modelParking.transform.Find("modelCar");
        playerBehindCarCamera = playerCar.Find("BehindCarCamera");
        modelBehindCarCamera = modelCar.Find("BehindCarCamera");
        Assert.AreEqual(1, dropdown.value);
        Assert.IsFalse(playerOverheadCamera.gameObject.activeSelf); // They shouldn't be active, so they shouldn't be found, so they should be null
        Assert.IsFalse(modelOverheadCamera.gameObject.activeSelf);
        Assert.IsTrue(playerBehindCarCamera.gameObject.activeSelf); // They should be active
        Assert.IsTrue(modelBehindCarCamera.gameObject.activeSelf);
    }
    [UnityTest]
    public IEnumerator CheckRaceParallelSameParkingTest()
    {
        // Arrange
        float epsilon = 0.01f;
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;

        var playerCar = GameObject.Find("playerCar");
        var modelCar = GameObject.Find("modelCar");
        var playerScript = playerCar.GetComponent<PlayerRaceCarAgent>();
        var modelScript = modelCar.GetComponent<ModelRaceCarAgent>();
        var playerCarZ = playerCar.transform.localPosition.z;
        var modelCarZ = modelCar.transform.localPosition.z;
        var playerParkingSlots = playerScript.ParkingSlots;
        var modelParkingSlots = playerScript.ParkingSlots;

        // Assert
        Assert.IsTrue(playerCarZ < modelCarZ + epsilon);
        Assert.IsTrue(playerCarZ > modelCarZ - epsilon);
        Assert.AreEqual(playerParkingSlots, modelParkingSlots);
    }
    [UnityTest]
    public IEnumerator CheckRaceSequentialSetStart()
    {
        // Arrange
        float epsilon = 0.01f;
        float carStart = -8f;
        yield return SceneManager.LoadSceneAsync("ChooseMap"); // Let the MapLoadVarsSingleton singleton instantiate
        yield return null; // Let the scene load
        MapLoadVarsSingleton.Instance.sceneName = sceneName;
        MapLoadVarsSingleton.Instance.modelInfo = new ModelInfo(modelName);
        MapLoadVarsSingleton.Instance.loadOnlyOnce = false;
        yield return SceneManager.LoadSceneAsync(overlayName); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the race start
        var raceModeDropdown = GameObject.Find("RaceModeSettings").GetComponentInChildren<TMP_Dropdown>();
        raceModeDropdown.value = 1; // Parallel racing
        yield return null; // Let the parkings change;

        var gameObject = GameObject.Find("StartPosition");
        var toggle = gameObject.GetComponentInChildren<Toggle>();
        var slider = gameObject.GetComponentInChildren<Slider>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var playerCar = GameObject.Find("playerCar");
        var modelCar = GameObject.Find("modelCar");
        
        Assert.AreEqual(false, toggle.isOn); // This toggle should be 'off' by default
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
            Assert.IsTrue(playerCar.transform.localPosition.z < carStart + epsilon); // Check whether the car is close enough to the requested start position
            Assert.IsTrue(modelCar.transform.localPosition.z > carStart - epsilon); // Check whether the car is close enough to the requested start position
        }
    }
    private int countParallelActiveParkedCars(string parkingName)
    {
        string parkingSlotName = "parkingSlot";
        string parkingAreaName = "parkingArea";
        string staticCarName = "static-car";
        int i = 1;
        int carCount = 0;
        var parallelParking = GameObject.Find(parkingName);
        if (parallelParking == null || !parallelParking.activeSelf) return 0;
        var parkingArea = parallelParking.transform.Find(parkingAreaName);
        if (parkingArea == null || !parallelParking.activeSelf) return 0;
        var gameObject = parkingArea.Find(parkingSlotName).gameObject;
        if (gameObject == null || !parallelParking.activeSelf) return 0;
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
