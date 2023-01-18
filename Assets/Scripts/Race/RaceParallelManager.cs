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
    [SerializeField] private Camera m_playerOverheadCamera;
    [SerializeField] private Camera m_playerBehindCarCamera;
    [SerializeField] private Camera m_modelOverheadCamera;
    [SerializeField] private Camera m_modelBehindCarCamera;
    private int racesComplete;
    private void Start()
    {
        racesComplete = 0;
        RaceSettingsSingleton.Instance.parkingGenerated = false;
    }
    public void manuallyRestarted()
    {
        racesComplete = 0;
        if (!m_playerCar.gameObject.activeSelf) m_playerCar.gameObject.SetActive(true);
        if (!m_modelCar.gameObject.activeSelf) m_modelCar.gameObject.SetActive(true);

        m_playerOverheadCamera.rect = new Rect(0, 0, 0.5f, 1);
        m_playerBehindCarCamera.rect = new Rect(0, 0, 0.5f, 1);
        m_modelOverheadCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
        m_modelBehindCarCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
    }
    public void finishedParking(CarOwner whichCar)
    {
        racesComplete++;
        switch(whichCar)
        {
            case CarOwner.Player:
                m_playerCar.gameObject.SetActive(false);
                m_playerOverheadCamera.rect = new Rect(0, 0, 0, 0);
                m_playerBehindCarCamera.rect = new Rect(0, 0, 0, 0);
                m_modelOverheadCamera.rect = new Rect(0, 0, 1, 1);
                m_modelBehindCarCamera.rect = new Rect(0, 0, 1, 1);
                break;
            case CarOwner.Model:
                m_modelCar.gameObject.SetActive(false);
                m_playerOverheadCamera.rect = new Rect(0, 0, 1, 1);
                m_playerBehindCarCamera.rect = new Rect(0, 0, 1, 1);
                m_modelOverheadCamera.rect = new Rect(0, 0, 0, 0);
                m_modelBehindCarCamera.rect = new Rect(0, 0, 0, 0);
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
