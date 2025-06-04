using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;
    public int CurrentShipID { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (!PlayerPrefs.HasKey("SHIP_0"))
        {
            PlayerPrefs.SetInt("SHIP_0", 1);
        }
    }

    public void SetCurrentShipID(int id)
    {
        CurrentShipID = id;
    }
}
