using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSetter : MonoBehaviour
{
    public int ItemID;
    public Image image;
    public TextMeshProUGUI PriceText;
    public GameObject ManageShop;

    public void Update() {
        PriceText.SetText(GameManager.instance.shopLists[ItemID].Price.ToString());
        image.sprite = GameManager.instance.shopLists[ItemID].image;
    }
}
