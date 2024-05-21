using UnityEngine;

public class InventorySetter : MonoBehaviour
{
    public Transform itemsParent;
    public InventorySlot[] allSlots;
    
    private void Start() {
        allSlots = itemsParent.GetComponentsInChildren<InventorySlot>();

        if( Inventory.instance.shopItems.Count != 0) {
            UpdateInventory();
        }
    }

    private void Update() {
        if(Inventory.instance.itemChanged) {
            UpdateInventory();
            Inventory.instance.itemChanged = false;
        }
    }

    private void UpdateInventory() {
        for(int i = 0; i < allSlots.Length; i++) {
            if(i < Inventory.instance.shopItems.Count) {
                allSlots[i].AddsItem(Inventory.instance.shopItems[i]);
            } else {
                allSlots[i].ClearSlot();
            }
        }
    }
}
