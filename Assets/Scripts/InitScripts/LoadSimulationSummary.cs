using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoadSimulationSummary : MonoBehaviour
{
    [SerializeField] private Transform m_simulationTimeTransform;
    [SerializeField] private Transform m_parkingSuccessTransform;
    void Start()
    {
        var simulationTimeText = m_simulationTimeTransform.GetComponent<TMP_Text>();
        TimeSpan ts = SimulationSummaryStaticVars.simulationEnd - SimulationSummaryStaticVars.simulationStart;
        int minutes = ts.Minutes;
        int seconds = ts.Seconds;
        int miliseconds = ts.Milliseconds;
        simulationTimeText.text = string.Format("{0}:{1}:{2}", minutes.ToString(), seconds.ToString(), miliseconds.ToString("##"));

        var parkingSuccessText = m_parkingSuccessTransform.GetComponent<TMP_Text>();
        if (SimulationSummaryStaticVars.parkingSuccessful) parkingSuccessText.text = "Tak";
        else parkingSuccessText.text = "Nie";
    }
}
