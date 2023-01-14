using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void MenuToMapChoice()
    {
        SceneManager.LoadScene("ChooseMap");
    }
}
