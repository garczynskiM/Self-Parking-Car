using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ObstacleResetTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ObstacleResetTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ObstacleResetTestWithEnumeratorPasses()
    {
        SceneManager.LoadScene("ChooseMap");
        yield return null;
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
    }
}
