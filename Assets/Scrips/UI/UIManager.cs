﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public const string COIN = "COIN";
    public static UIManager instance;
    public Text coinText;

    public GameObject waveUI;
    public Text waveText;

    public GameObject homePanel;
    public GameObject shopPanel;
    public GameObject upgradePanel;
    public GameObject playGameUI;
    public GameObject winUI;
    public GameObject loseUI;

    public GameObject topBar;

    private List<GameObject> allPanels;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        allPanels = new List<GameObject> { homePanel, shopPanel, upgradePanel, playGameUI, winUI, loseUI };
    }

    private void Start()
    {
        UpdateCoinUI();
        ShowOnlyPanel(homePanel);
    }

    public void PlayGame()
    {
        ShowOnlyPanel(playGameUI);
        LevelManager.Instance.LoadSavedLevel();
        topBar.SetActive(false);
    }

    public void Next()
    {
        LevelManager.Instance.NextLevel();
    }

    public void Home()
    {
        ShowOnlyPanel(homePanel);
        LevelManager.Instance.DestroyCurrentLevel();
        LevelManager.Instance.DestroySpawnedShips();
    }

    public void Replay()
    {
        loseUI.SetActive(false);
        LevelManager.Instance.ReplayLevel();
    }
    public void UpdateCoinUI()
    {
        coinText.text = Coin.ToString();
    }

    public void ShowWaveUI(int currentWave, int totalWaves)
    {
        waveText.text = "Đợt " + currentWave + "/" + totalWaves;
        waveUI.SetActive(true);
    }

    public void HideWaveUI()
    {
        waveUI.SetActive(false);
    }

    int coint;
    public int Coin
    {
        get
        {
            int coint = PlayerPrefs.GetInt(COIN, 10000);
            return coint;
        }
        set
        {
            coint = value;
            PlayerPrefs.SetInt(COIN, value);
            PlayerPrefs.Save();
        }
    }

    public void ShowOnlyPanel(GameObject panelToShow)
    {
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(panel == panelToShow);
        }
    }

    public void ShowHome() => ShowOnlyPanel(homePanel);
    public void ShowShop() => ShowOnlyPanel(shopPanel);
    public void ShowUpgrade() => ShowOnlyPanel(upgradePanel);

}
