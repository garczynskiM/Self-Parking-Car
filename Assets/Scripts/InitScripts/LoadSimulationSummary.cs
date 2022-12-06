using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadSimulationSummary : MonoBehaviour
{
    [SerializeField] private Transform m_simulationTimeTransform;
    [SerializeField] private Transform m_parkingSuccessTransform;
    void Start()
    {
        Text simulationTimeText = m_simulationTimeTransform.GetComponent<Text>();
        TimeSpan ts = SimulationSummaryStaticVars.simulationEnd - SimulationSummaryStaticVars.simulationStart;
        int minutes = ts.Minutes;
        int seconds = ts.Seconds;
        int miliseconds = ts.Milliseconds;
        simulationTimeText.text = string.Format("{0}:{1}:{2}", minutes.ToString(), seconds.ToString(), miliseconds.ToString());

        Text parkingSuccessText = m_parkingSuccessTransform.GetComponent<Text>();
        if (SimulationSummaryStaticVars.parkingSuccessful) parkingSuccessText.text = "Tak";
        else parkingSuccessText.text = "Nie";
    }
}
