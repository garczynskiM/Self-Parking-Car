using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppSettingsDropdown : MonoBehaviour
{
    public void DropdownChangeValue(TMP_Dropdown change)
    {
        switch(change.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            default:
                break;
        }

    }
}
