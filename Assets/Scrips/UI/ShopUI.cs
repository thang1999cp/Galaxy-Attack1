using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public int shipID;
    public int price = 1000;  
    public Button buyButton; 
    public GameObject lockIcon;  
    public Text priceText; 

    private string unlockKey => "ShipUnlocked_" + shipID;

    void Start()
    {
        buyButton.onClick.AddListener(BuyShip);
        UpdateUI();
    }

    void UpdateUI()
    {
        bool isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;

        lockIcon.SetActive(!isUnlocked);
        priceText.text = isUnlocked ? "Owned" : price.ToString();
        buyButton.interactable = !isUnlocked;
    }

    void BuyShip()
    {
        if (PlayerPrefs.GetInt(unlockKey, 0) == 1) return;

        if (UIManager.instance.Coin >= price)
        {
            UIManager.instance.Coin -= price;
            UIManager.instance.UpdateCoinUI();

            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();

            UpdateUI();
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
}
