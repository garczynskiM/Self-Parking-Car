using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AppSettingsButton : MonoBehaviour
{
    public void saveSettings()
    {
        AppSettingsStaticVars.saveScreenSettings();
        SceneManager.LoadScene("Menu");
    }
    public void goToAppSettings()
    {
        SceneManager.LoadScene("AppSettings");
    }
}
