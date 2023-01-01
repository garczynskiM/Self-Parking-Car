using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadRaceSummary : MonoBehaviour
{
    [SerializeField] private Transform m_playerTimeTransform;
    [SerializeField] private Transform m_playerParkingSuccessTransform;
    [SerializeField] private Transform m_carTimeTransform;
    [SerializeField] private Transform m_carParkingSuccessTransform;
    void Start()
    {
        var playerTimeText = m_playerTimeTransform.GetComponent<TMP_Text>();
        TimeSpan ts = SimulationSummaryStaticVars.simulationEnd - SimulationSummaryStaticVars.simulationStart;
        int minutes = ts.Minutes;
        int seconds = ts.Seconds;
        int miliseconds = ts.Milliseconds;
        playerTimeText.text = string.Format("{0}:{1}:{2}", minutes.ToString(), seconds.ToString(), miliseconds.ToString("##"));

        var playerParkingSuccessText = m_playerParkingSuccessTransform.GetComponent<TMP_Text>();
        if (SimulationSummaryStaticVars.parkingSuccessful) playerParkingSuccessText.text = "Tak";
        else playerParkingSuccessText.text = "Nie";

        var carTimeText = m_carTimeTransform.GetComponent<TMP_Text>();
        ts = SimulationSummaryStaticVars.simulationEnd - SimulationSummaryStaticVars.simulationStart;
        minutes = ts.Minutes;
        seconds = ts.Seconds;
        miliseconds = ts.Milliseconds;
        carTimeText.text = string.Format("{0}:{1}:{2}", minutes.ToString(), seconds.ToString(), miliseconds.ToString("##"));

        var carParkingSuccessText = m_carParkingSuccessTransform.GetComponent<TMP_Text>();
        if (SimulationSummaryStaticVars.parkingSuccessful) carParkingSuccessText.text = "Tak";
        else carParkingSuccessText.text = "Nie";
    }
}
