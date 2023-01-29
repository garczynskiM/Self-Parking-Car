using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaceSetEndSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Slider slider;
    public void OnPointerDown(PointerEventData eventData)
    {
        RaceSettingsSingleton.Instance.startSetEnd((int)slider.value);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        RaceSettingsSingleton.Instance.endSetEnd((int)slider.value);
    }

    public void valueChanged()
    {
        RaceSettingsSingleton.Instance.updateSetEnd((int)slider.value);
    }
}
