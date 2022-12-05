using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppSettingsScript : MonoBehaviour
{
    [SerializeField] private Transform m_FullscreenToggle;
    // Start is called before the first frame update
    void Start()
    {
        var toggle = m_FullscreenToggle.GetComponentInChildren<Toggle>();
        toggle.isOn = Screen.fullScreen;
    }
}
