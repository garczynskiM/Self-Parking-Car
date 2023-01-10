using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppSettingsScript : MonoBehaviour
{
    [SerializeField] private Transform m_FullscreenToggle;
    [SerializeField] private Transform m_resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        AppSettingsStaticVars.saveScreenSettings();
        var toggle = m_FullscreenToggle.GetComponentInChildren<Toggle>();
        toggle.isOn = Screen.fullScreen;
        var dropdown = m_resolutionDropdown.GetComponent<TMP_Dropdown>();
        switch(Screen.width, Screen.height)
        {
            case (1920, 1080):
                dropdown.value = 0;
                break;
            case (1280, 720):
                dropdown.value = 1;
                break;
            default:
                break;
        }
    }
}
