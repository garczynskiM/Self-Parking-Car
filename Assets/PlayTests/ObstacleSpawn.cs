using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ObstacleSpawn
{
    // A Test behaves as an ordinary method
    [Test]
    public void ObstacleSpawnSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ObstacleSpawnWithEnumeratorPasses()
    {
        MapLoadStaticVars.sceneName = "Parallel";
        SceneManager.LoadScene("SimulationOverlay");
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
