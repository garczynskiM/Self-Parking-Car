using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CarOwner
{
    Player,
    Model,
    None
}
public class RaceParallelManager : MonoBehaviour
{
    [SerializeField] private Transform m_playerCar;
    [SerializeField] private Transform m_modelCar;
    private int racesComplete;
    private void Start()
    {
        racesComplete = 0;
        RaceSettingsSingleton.Instance.parkingGenerated = false;
    }
    public void manuallyRestarted()
    {
        racesComplete = 0;
        if(!m_playerCar.gameObject.activeSelf) m_playerCar.gameObject.SetActive(true);
        if (!m_modelCar.gameObject.activeSelf) m_modelCar.gameObject.SetActive(true);
    }
    public void finishedParking(CarOwner whichCar)
    {
        racesComplete++;
        switch(whichCar)
        {
            case CarOwner.Player:
                m_playerCar.gameObject.SetActive(false);
                break;
            case CarOwner.Model:
                m_modelCar.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        if(racesComplete == 2)
        {
            SceneManager.LoadScene("RaceSummary");
        }
    }
}
