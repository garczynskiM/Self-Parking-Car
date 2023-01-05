using System.Collections;
using System.Collections.Generic;
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
    public static bool manualRestart = false;
    public static GameObject overheadCamera;
    public static GameObject behindCarCamera;
}
