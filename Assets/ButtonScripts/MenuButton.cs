using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    //[SerializeField] private co� co�
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        SceneManager.LoadScene("Assets/Scenes/ChooseMap.unity");
    }
}
