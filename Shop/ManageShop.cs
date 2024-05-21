using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ManageShop : MonoBehaviour
{
    public TextMeshProUGUI svcoinText;
    public TextMeshProUGUI storageText;
    public GameObject fundsPopUp;
    public GameObject inventoryPopUp;
    private Inventory inventory;

    private void Start() {
        svcoinText.SetText(GameManager.instance.Svcoin.ToString());
        storageText.SetText(GameManager.instance.Storage.ToString() + "/" + GameManager.instance.MaxStorage.ToString());
    }

    private void Update() {
        svcoinText.SetText(GameManager.instance.Svcoin.ToString());
        storageText.SetText(GameManager.instance.Storage.ToString() + "/" + GameManager.instance.MaxStorage.ToString());
    }

    public void Buy() {
        GameObject Item = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ShopItem shopItem = GameManager.instance.shopLists[Item.GetComponent<ShopSetter>().ItemID]; 

        fundsPopUp.SetActive(false);
        inventoryPopUp.SetActive(false);

        if(GameManager.instance.Svcoin >= GameManager.instance.shopLists[Item.GetComponent<ShopSetter>().ItemID].Price) {
            if(GameManager.instance.Storage < GameManager.instance.MaxStorage) {
                GameManager.instance.Svcoin -=  GameManager.instance.shopLists[Item.GetComponent<ShopSetter>().ItemID].Price;
                GameManager.instance.Storage++;

                Inventory.instance.AddItem(shopItem);
                Debug.Log(shopItem.image);
            } else {
                inventoryPopUp.SetActive(true);
            }
        } else {
            fundsPopUp.SetActive(true);
        }
    }
}
