using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject homePanel;
    [SerializeField]
    private GameObject playPanel;
    [SerializeField]
    private GameObject customPanel;
    [SerializeField]
    private GameObject statsPanel;
    [SerializeField]
    private GameObject confirmationPanel;

    private void Start()
    {
        EnableHomePanel();

        SetupStats();
    }

    public void EnableHomePanel()
    {
        homePanel.SetActive(true);
        playPanel.SetActive(false);
        statsPanel.SetActive(false);
        confirmationPanel.SetActive(false);
    }

    public void EnablePlayPanel()
    {
        homePanel.SetActive(false);
        playPanel.SetActive(true);
        customPanel.SetActive(false);
    }

    public void EnableCustomPanel()
    {
        homePanel.SetActive(false);
        playPanel.SetActive(false);
        customPanel.SetActive(true);
    }

    public void EnableStatsPanel()
    {
        homePanel.SetActive(false);
        statsPanel.SetActive(true);
        confirmationPanel.SetActive(false);
    }

    public void EnableConfirmationPanel()
    {
        statsPanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    private void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void SetupStats()
    {

    }
}
