using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image sItemImage;
    public TextMeshProUGUI nameText;
    private ShopItem sItem;
    public Button removeBtn;

    public void AddsItem (ShopItem newsItem) {
        sItem = newsItem;
        sItemImage.sprite = sItem.image;
        Debug.Log(sItemImage);
        sItemImage.enabled = true;
        removeBtn.interactable = true;
        nameText.SetText(sItem.ItemName);
    }

    public void ClearSlot() {
        sItem = null;
        sItemImage.sprite = null;
        Debug.Log(sItemImage);
        sItemImage.enabled = false;
        removeBtn.interactable = false;
        nameText.SetText("");
    }

    public void OnRemoveButton() {
        Inventory.instance.RemoveItem(sItem);
    }
}
