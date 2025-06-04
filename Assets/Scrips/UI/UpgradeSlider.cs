using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    Power,
    Bullet
}

public class UpgradeSlider : MonoBehaviour
{
    public GameObject[] fills;
    public Button upgradeButton;
    public int baseCost = 1000;

    public UpgradeType upgradeType; 

    public Text costText;

    private int currentLevel = 0;
    private int upgradeCost;

    [Header("Plane Settings")]
    public int targetPlaneId = 1;
    private void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
        RefreshUpgradeData();
    }

    public void RefreshUpgradeData()
    {
        targetPlaneId = ShipManager.Instance.CurrentShipID;

        Plane plane = PlaneManager.Instance.GetPlaneById(targetPlaneId);
        if (plane == null) return;

        currentLevel = PlayerPrefs.GetInt(GetLevelKey(), 0);
        upgradeCost = PlayerPrefs.GetInt(GetCostKey(), baseCost);

        UpdateFillUI();
        UpdateCostUI();
    }

    void UpdateCostUI()
    {
        if (costText != null)
            costText.text = upgradeCost.ToString();
    }

    public void Upgrade()
    {
        if (!IsShipBought()) return;

        if (currentLevel < fills.Length && UIManager.instance.Coin >= upgradeCost)
        {
            UIManager.instance.Coin -= upgradeCost;
            UIManager.instance.UpdateCoinUI();

            currentLevel++;
            ApplyUpgradeEffect();

            upgradeCost += 500;
            SaveUpgrade();

            UpdateFillUI();
            UpdateCostUI();

            PlaneManager.Instance.RefreshUpgradeData();
        }
    }

    private bool IsShipBought()
    {
        return PlayerPrefs.GetInt("SHIP_" + targetPlaneId, 0) == 1;
    }

    void ApplyUpgradeEffect()
    {
        Plane p = PlaneManager.Instance.GetPlaneById(targetPlaneId);
        if (p == null) return;

        switch (upgradeType)
        {
            case UpgradeType.Power:
                p.power = p.basePower + currentLevel;
                Debug.Log($"Tăng sát thương cho plane {targetPlaneId}! Power = {p.power}");
                break;
            case UpgradeType.Bullet:
                p.quantityBullet = p.baseQuantityBullet + currentLevel;
                Debug.Log($"Tăng đạn cho plane {targetPlaneId}! Bullets = {p.quantityBullet}");
                break;
        }
    }

    void UpdateFillUI()
    {
        for (int i = 0; i < fills.Length; i++)
        {
            fills[i].SetActive(i < currentLevel);
        }
    }

    void SaveUpgrade()
    {
        PlayerPrefs.SetInt(GetLevelKey(), currentLevel);
        PlayerPrefs.SetInt(GetCostKey(), upgradeCost);
        PlayerPrefs.Save();
    }

    string GetLevelKey()
    {
        return "UPGRADE_LEVEL_" + upgradeType + "_" + targetPlaneId;
    }

    string GetCostKey()
    {
        return "UPGRADE_COST_" + upgradeType + "_" + targetPlaneId;
    }
}
