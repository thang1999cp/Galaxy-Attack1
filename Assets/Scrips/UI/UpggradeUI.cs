using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpggradeUI : MonoBehaviour
{
    public List<GameObject> ships;
    public Button btnLeft;
    public Button btnRight;

    private List<int> unlockedShipIDs = new List<int>();
    private int currentIndex = 0;

    private void OnEnable()
    {
        BtnBuyShip.OnShipBought += OnShipBought;
    }

    private void OnDisable()
    {
        BtnBuyShip.OnShipBought -= OnShipBought;
    }

    private void OnShipBought(int boughtShipId)
    {
        RefreshUnlockedShips();
        InitDisplay();
    }

    void Start()
    {
        RefreshUnlockedShips();
        LoadSelectedShip();
        InitDisplay();
        btnLeft.onClick.AddListener(OnLeft);
        btnRight.onClick.AddListener(OnRight);
    }

    public void RefreshUnlockedShips()
    {
        unlockedShipIDs.Clear();

        for (int i = 0; i < ships.Count; i++)
        {
            if (PlayerPrefs.GetInt("SHIP_" + i, 0) == 1)
            {
                unlockedShipIDs.Add(i);
            }
        }
    }

    private void LoadSelectedShip()
    {
        int savedShipID = PlayerPrefs.GetInt("SELECTED_SHIP_ID", -1);

        if (savedShipID != -1 && unlockedShipIDs.Contains(savedShipID))
        {
            currentIndex = unlockedShipIDs.IndexOf(savedShipID);
        }
        else
        {
            currentIndex = 0;
            if (unlockedShipIDs.Count > 0)
            {
                PlayerPrefs.SetInt("SELECTED_SHIP_ID", unlockedShipIDs[0]);
                PlayerPrefs.Save();
            }
        }
    }
    public void InitDisplay()
    {
        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].SetActive(false);
        }

        if (unlockedShipIDs.Count > 0)
        {
            int activeShipID = unlockedShipIDs[currentIndex];
            ships[activeShipID].SetActive(true);
            ShipManager.Instance.SetCurrentShipID(activeShipID);
            PlaneManager.Instance.RefreshUpgradeData();
        }

        UpdateArrowButtons();
    }

    void OnLeft()
    {
        if (currentIndex <= 0) return;

        int currentID = unlockedShipIDs[currentIndex];
        ships[currentID].SetActive(false);

        currentIndex--;
        int newID = unlockedShipIDs[currentIndex];

        PlayerPrefs.SetInt("SELECTED_SHIP_ID", newID);
        PlayerPrefs.Save();

        ships[newID].SetActive(true);
        ShipManager.Instance.SetCurrentShipID(newID);
        PlaneManager.Instance.RefreshUpgradeData();
        UpdateArrowButtons();
        RefreshSliders();
    }

    void OnRight()
    {
        if (currentIndex >= unlockedShipIDs.Count - 1) return;

        int currentID = unlockedShipIDs[currentIndex];
        ships[currentID].SetActive(false);

        currentIndex++;
        int newID = unlockedShipIDs[currentIndex];

        PlayerPrefs.SetInt("SELECTED_SHIP_ID", newID);
        PlayerPrefs.Save();

        ships[newID].SetActive(true);
        ShipManager.Instance.SetCurrentShipID(newID);
        PlaneManager.Instance.RefreshUpgradeData();
        UpdateArrowButtons();
        RefreshSliders();
    }

    void UpdateArrowButtons()
    {
        btnLeft.gameObject.SetActive(currentIndex > 0);
        btnRight.gameObject.SetActive(currentIndex < unlockedShipIDs.Count - 1);
    }

    void RefreshSliders()
    {
        UpgradeSlider[] sliders = FindObjectsOfType<UpgradeSlider>();
        foreach (var slider in sliders)
        {
            slider.RefreshUpgradeData();
        }
    }
}
