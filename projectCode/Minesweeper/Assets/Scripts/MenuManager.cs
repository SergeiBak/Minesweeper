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

    [SerializeField]
    private Text beginnerTime;
    [SerializeField]
    private Text standardTime;
    [SerializeField]
    private Text advancedTime;

    private int customWidth = 16;
    private int customHeight = 16;
    private int customMines = 40;

    [SerializeField]
    private Text widthLabel;
    [SerializeField]
    private Text heightLabel;
    [SerializeField]
    private Text minesLabel;

    [SerializeField]
    private Slider minesSlider;
    [SerializeField]
    private Text maxMinesText;

    private void Start()
    {
        EnableHomePanel();

        SetupStats();
    }

    public void EnableHomePanel()
    {
        homePanel.SetActive(true);
        playPanel.SetActive(false);
        customPanel.SetActive(false);
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
        homePanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    private void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PlayBeginner()
    {
        PlayerPrefs.SetInt("MinesweeperGameMode", 1);

        PlayerPrefs.SetInt("MinesweeperWidth", 9);
        PlayerPrefs.SetInt("MinesweeperHeight", 9);
        PlayerPrefs.SetInt("MinesweeperMines", 10);

        Play();
    }

    public void PlayStandard()
    {
        PlayerPrefs.SetInt("MinesweeperGameMode", 2);

        PlayerPrefs.SetInt("MinesweeperWidth", 16);
        PlayerPrefs.SetInt("MinesweeperHeight", 16);
        PlayerPrefs.SetInt("MinesweeperMines", 40);

        Play();
    }

    public void PlayAdvanced()
    {
        PlayerPrefs.SetInt("MinesweeperGameMode", 3);

        PlayerPrefs.SetInt("MinesweeperWidth", 24);
        PlayerPrefs.SetInt("MinesweeperHeight", 20);
        PlayerPrefs.SetInt("MinesweeperMines", 99);

        Play();
    }

    public void PlayCustom()
    {
        PlayerPrefs.SetInt("MinesweeperGameMode", 0);

        PlayerPrefs.SetInt("MinesweeperWidth", customWidth);
        PlayerPrefs.SetInt("MinesweeperHeight", customHeight);
        PlayerPrefs.SetInt("MinesweeperMines", customMines);

        Play();
    }

    private void SetupStats()
    {
        if (!PlayerPrefs.HasKey("MinesweeperBestBeginnerTime"))
        {
            PlayerPrefs.SetInt("MinesweeperBestBeginnerTime", 999);
        }
        if (!PlayerPrefs.HasKey("MinesweeperBestStandardTime"))
        {
            PlayerPrefs.SetInt("MinesweeperBestStandardTime", 999);
        }
        if (!PlayerPrefs.HasKey("MinesweeperBestAdvancedTime"))
        {
            PlayerPrefs.SetInt("MinesweeperBestAdvancedTime", 999);
        }

        if (!PlayerPrefs.HasKey("MinesweeperGameMode"))
        {
            PlayerPrefs.SetInt("MinesweeperGameMode", 2);
        }

        if (!PlayerPrefs.HasKey("MinesweeperWidth"))
        {
            PlayerPrefs.SetInt("MinesweeperWidth", 16);
        }
        if (!PlayerPrefs.HasKey("MinesweeperHeight"))
        {
            PlayerPrefs.SetInt("MinesweeperHeight", 16);
        }
        if (!PlayerPrefs.HasKey("MinesweeperMines"))
        {
            PlayerPrefs.SetInt("MinesweeperMines", 40);
        }

        beginnerTime.text = "Beginner: " + PlayerPrefs.GetInt("MinesweeperBestBeginnerTime").ToString();
        standardTime.text = "Standard: " + PlayerPrefs.GetInt("MinesweeperBestStandardTime").ToString();
        advancedTime.text = "Advanced: " + PlayerPrefs.GetInt("MinesweeperBestAdvancedTime").ToString();
    }

    public void DeleteStats()
    {
        PlayerPrefs.DeleteAll();

        SetupStats();

        EnableHomePanel();
    }

    public void UpdateWidthValue(float val)
    {
        customWidth = (int)val;

        widthLabel.text = "Width: " + val.ToString();

        UpdateMaxMinesSlider();
    }

    public void UpdateHeightValue(float val)
    {
        customHeight = (int)val;

        heightLabel.text = "Height: " + val.ToString();

        UpdateMaxMinesSlider();
    }

    public void UpdateMinesValue(float val)
    {
        customMines = (int)val;

        minesLabel.text = "Mines: " + val.ToString();

        UpdateMaxMinesSlider();
    }

    private void UpdateMaxMinesSlider()
    {
        int maxMines = (customHeight * customWidth) - 9;
        if (maxMines < 0)
        {
            maxMines = 0;
        }
        minesSlider.maxValue = maxMines;

        if (customMines > maxMines)
        {
            customMines = maxMines;
        }

        maxMinesText.text = maxMines.ToString();
    }
}
