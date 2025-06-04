using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnBuyShip : MonoBehaviour
{
    public static event System.Action<int> OnShipBought;

    public int shipID;
    [SerializeField] private int price = 2000;
    public Button buyButton;
    public Text priceText;

    private void Start()
    {
        priceText.text = price.ToString();

        if (IsShipBought())
        {
            buyButton.interactable = false;
            priceText.text = "Đã mua";
        }

        buyButton.onClick.AddListener(BuyShip);
    }

    private void BuyShip()
    {
        if (UIManager.instance.Coin >= price)
        {
            UIManager.instance.Coin -= price;
            PlayerPrefs.SetInt(GetShipKey(), 1);
            UIManager.instance.UpdateCoinUI();

            buyButton.interactable = false;
            priceText.text = "Đã mua";
            Debug.Log("Mua thành công máy bay ID: " + shipID);

            OnShipBought?.Invoke(shipID);
        }
        else
        {
            Debug.Log("Không đủ coin để mua!");
        }
    }

    private bool IsShipBought()
    {
        return PlayerPrefs.GetInt(GetShipKey(), 0) == 1;
    }

    private string GetShipKey()
    {
        return "SHIP_" + shipID;
    }
}
