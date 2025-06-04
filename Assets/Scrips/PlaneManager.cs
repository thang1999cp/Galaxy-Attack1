using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Plane
{
    public int id;
    public GameObject plane;
    public int power;
    public int quantityBullet;

    [HideInInspector] public int basePower;
    [HideInInspector] public int baseQuantityBullet;
}
public class PlaneManager : MonoBehaviour
{
    public static PlaneManager Instance;
    public List<Plane> plane;

    private void Awake()
    {
        Instance = this;
        InitializePlanes();
    }

    private void Start()
    {
        LoadUpgradeData();
    }

    void InitializePlanes()
    {
        foreach (Plane p in plane)
        {
            string basePowerKey = "BASE_POWER_" + p.id;
            string baseBulletKey = "BASE_BULLET_" + p.id;

            if (!PlayerPrefs.HasKey(basePowerKey))
            {
                PlayerPrefs.SetInt(basePowerKey, p.power);
                p.basePower = p.power;
            }
            else
            {
                p.basePower = PlayerPrefs.GetInt(basePowerKey);
            }

            if (!PlayerPrefs.HasKey(baseBulletKey))
            {
                PlayerPrefs.SetInt(baseBulletKey, p.quantityBullet);
                p.baseQuantityBullet = p.quantityBullet;
            }
            else
            {
                p.baseQuantityBullet = PlayerPrefs.GetInt(baseBulletKey);
            }
        }

        PlayerPrefs.Save();
    }

    void LoadUpgradeData()
    {
        foreach (Plane p in plane)
        {
            string powerLevelKey = "UPGRADE_LEVEL_Power_" + p.id;
            int powerLevel = PlayerPrefs.GetInt(powerLevelKey, 0);
            p.power = p.basePower + powerLevel;

            string bulletLevelKey = "UPGRADE_LEVEL_Bullet_" + p.id;
            int bulletLevel = PlayerPrefs.GetInt(bulletLevelKey, 0);
            p.quantityBullet = p.baseQuantityBullet + bulletLevel;
        }
    }

    public Plane GetPlaneById(int id)
    {
        return plane.Find(p => p.id == id);
    }

    public void RefreshUpgradeData()
    {
        LoadUpgradeData();
    }
}