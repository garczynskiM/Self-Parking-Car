using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AppSettingsButton : MonoBehaviour
{
    [SerializeField] private Transform m_FullscreenToggle;
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
