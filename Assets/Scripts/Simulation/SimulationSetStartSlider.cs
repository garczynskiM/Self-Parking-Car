using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimulationSetStartSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SimulationSettingsSingleton.Instance.startMarker.gameObject.SetActive(true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        SimulationSettingsSingleton.Instance.startMarker.gameObject.SetActive(false);
    }

    public void valueChanged(Slider slider)
    {
        float currentZ = SimulationSettingsSingleton.Instance.startMarker.localPosition.z;
        SimulationSettingsSingleton.Instance.startMarker.localPosition = 
            SimulationSettingsSingleton.Instance.startMarker.localPosition + new Vector3(0, 0, slider.value - currentZ);
        SimulationSettingsSingleton.Instance.setStartZ = slider.value;
    }
}
