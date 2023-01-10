using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BackToMenuButton : MonoBehaviour
{
    public void backFromSettings()
    {
        AppSettingsStaticVars.restoreScreenSettings();
        SceneManager.LoadScene("Menu");
    }
    public void backFromMapChoice()
    {
        SceneManager.LoadScene("Menu");
    }
}
