using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Player Ship")]
    public Transform spawnPoint;

    [Header("Levels")]
    public GameObject[] levels;

    private int currentLevelIndex;
    private GameObject currentLevelInstance;
    private List<GameObject> spawnedShips = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadSavedLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL_INDEX", 0);

        SpawnLevel(currentLevelIndex);
        SpawnPlayerShip();
    }

    public void NextLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);  // Xóa level hiện tại
        }

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("Đã vượt qua tất cả các màn!");
            currentLevelIndex = 0;
        }

        PlayerPrefs.SetInt("CURRENT_LEVEL_INDEX", currentLevelIndex);
        PlayerPrefs.Save();

        SpawnLevel(currentLevelIndex);
        SpawnPlayerShip();
    }

    private void SpawnLevel(int index)
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        if (index >= 0 && index < levels.Length)
        {
            currentLevelInstance = Instantiate(levels[index]);
            currentLevelInstance.SetActive(true);
            Debug.Log("Đã spawn level " + index);
        }
        else
        {
            Debug.LogError("Level index không hợp lệ: " + index);
        }
    }

    private void SpawnPlayerShip()
    {
        int selectedID = PlayerPrefs.GetInt("SELECTED_SHIP_ID", 0);
        Plane selectedPlane = PlaneManager.Instance.GetPlaneById(selectedID);

        if (selectedPlane != null && selectedPlane.plane != null)
        {
            GameObject shipClone = Instantiate(selectedPlane.plane, spawnPoint.position, spawnPoint.rotation);
            shipClone.SetActive(true);

            spawnedShips.Add(shipClone);
        }

    }

    public void DestroyCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }
    }

    public void DestroySpawnedShips()
    {
        foreach (GameObject ship in spawnedShips)
        {
            if (ship != null)
                Destroy(ship);
        }
        spawnedShips.Clear();
    }
    private void CleanupCurrentLevel()
    {
        DestroyObjectsWithTag("Enemy");
        DestroyObjectsWithTag("Bullet");
    }
    public void ReplayLevel()
    {
        DestroyCurrentLevel();
        DestroySpawnedShips();

        SpawnLevel(currentLevelIndex);
        SpawnPlayerShip();
        CleanupCurrentLevel();
    }
    private void DestroyObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                Destroy(obj);
        }
        Debug.Log($"Destroyed {objects.Length} objects with tag: {tag}");
    }
}
