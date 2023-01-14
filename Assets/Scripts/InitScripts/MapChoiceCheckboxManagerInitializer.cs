using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChoiceCheckboxManagerInitializer : MonoBehaviour
{
    [SerializeField] private Transform m_mapCheckboxManager;
    // Start is called before the first frame update
    void Start()
    {
        ToggleManager script = m_mapCheckboxManager.GetComponent<ToggleManager>();
        script.startToggleManager();
    }
}
