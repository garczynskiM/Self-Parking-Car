using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimulationSetEndSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Slider slider;
    public void OnPointerDown(PointerEventData eventData)
    {
        SimulationSettingsSingleton.Instance.startSetEnd((int)slider.value);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        SimulationSettingsSingleton.Instance.endSetEnd((int)slider.value);
    }

    public void valueChanged()
    {
        SimulationSettingsSingleton.Instance.updateSetEnd((int)slider.value);
    }
}
