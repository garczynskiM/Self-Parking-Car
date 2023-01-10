using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FullscreenToggleManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    public void onValueChanged(Toggle toggle)
    {
        Screen.fullScreen = toggle.isOn;
    }
}
