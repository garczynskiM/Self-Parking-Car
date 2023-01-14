using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoadVarsSingleton : MonoBehaviour
{
    public string sceneName;
    public bool loadOnlyOnce;
    public ModelInfo modelInfo;
    public Transform m_autoRestartTransform;
    public Transform m_otherCarsTransform;

    public static MapLoadVarsSingleton Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
