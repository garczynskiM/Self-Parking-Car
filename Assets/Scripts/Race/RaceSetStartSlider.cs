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
