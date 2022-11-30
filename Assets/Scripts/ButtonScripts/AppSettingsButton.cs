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
        var toggle = m_FullscreenToggle.GetComponentInChildren<Toggle>();
        Screen.fullScreen = toggle.isOn;
        SceneManager.LoadScene("Menu");
    }
    public void goToAppSettings()
    {
        SceneManager.LoadScene("AppSettings");
    }
}
