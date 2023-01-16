using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RaceSetStartSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        RaceSettingsSingleton.Instance.activateStartMarker(true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        RaceSettingsSingleton.Instance.activateStartMarker(false);
    }

    public void valueChanged(Slider slider)
    {
        RaceSettingsSingleton.Instance.setStartZ = slider.value;
        RaceSettingsSingleton.Instance.setStartMarkerPosition(slider.value);
    }
}
