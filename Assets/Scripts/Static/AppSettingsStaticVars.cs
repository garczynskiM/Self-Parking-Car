using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppSettingsStaticVars
{
    public static int width;
    public static int height;
    public static bool fullscreen;
    public static void saveScreenSettings()
    {
        fullscreen = Screen.fullScreen;
        width = Screen.width;
        height = Screen.height;
    }
    public static void restoreScreenSettings()
    {
        Screen.SetResolution(Math.Max(width, 800), Math.Max(height, 600), fullscreen);
    }
}
