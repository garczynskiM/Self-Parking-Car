using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxManager : MonoBehaviour
{
    [SerializeField] private Transform m_ContentContainer;
    private Toggle[] m_Toggles;
    private int childCount;
    // Start is called before the first frame update
    void Start()
    {
        childCount = m_ContentContainer.childCount;
        m_Toggles = new Toggle[childCount];
        for (int i = 0; i < childCount; i++)
        {
            var mapItems = m_ContentContainer.GetChild(i);
            var toggle = mapItems.GetComponentInChildren<Toggle>();
            m_Toggles[i] = toggle;
            toggle.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggle);
            });
        }
    }

    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            for (int i = 0; i < childCount; i++)
            {
                if (!m_Toggles[i].Equals(change))
                {
                    m_Toggles[i].SetIsOnWithoutNotify(false);
                }
            }
        }
        else change.SetIsOnWithoutNotify(true);
    }
}
