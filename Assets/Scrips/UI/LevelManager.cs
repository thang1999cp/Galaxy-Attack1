using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Transform spawnPoint;
    private void Awake()
    {
        Instance = this;
    }
    public void Loadlevel()
    {
        int selectedID = PlayerPrefs.GetInt("SELECTED_SHIP_ID", 0);
        Plane selectedPlane = PlaneManager.Instance.GetPlaneById(selectedID);

        if (selectedPlane != null && selectedPlane.plane != null)
        {
            GameObject shipsClone = Instantiate(selectedPlane.plane, spawnPoint.position, spawnPoint.rotation);
            shipsClone.SetActive(true);

            Debug.Log("đã sinh ra máy bay" + transform.position);
        }
        else
        {
            Debug.LogError("Không thể sinh tàu: không tìm thấy plane với ID = " + selectedID);
        }
    }
}
