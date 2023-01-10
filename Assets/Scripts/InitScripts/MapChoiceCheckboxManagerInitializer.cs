using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChoiceCheckboxManagerInitializer : MonoBehaviour
{
    [SerializeField] private Transform m_mapCheckboxManager;
    // Start is called before the first frame update
    void Start()
    {
        CheckboxManager script = m_mapCheckboxManager.GetComponent<CheckboxManager>();
        script.startToggleManager();
    }
}
