using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RestartSimulationTest : MonoBehaviour
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CheckSimulationRestartTest()
    {
        // Arrange
        MapLoadStaticVars.sceneName = "Parallel";
        MapLoadStaticVars.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync("SimulationOverlay"); // Load the overlay
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
        MapLoadStaticVars.sceneName = "Parallel";
        MapLoadStaticVars.loadOnlyOnce = false;

        // Act
        yield return SceneManager.LoadSceneAsync("SimulationOverlay"); // Load the overlay
        yield return null; // Let the parking load
        yield return null; // Let the simulation start
        var toggle = GameObject.Find("OtherCars").GetComponent<Toggle>();
        var button = GameObject.Find("RestartAndApply").GetComponent<Button>();
        var script = GameObject.Find("car-root").GetComponent<SimulationCarAgent>();
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
