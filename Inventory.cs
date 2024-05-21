using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }
    public bool itemChanged;
    public List<ShopItem> shopItems = new List<ShopItem>();
    private PlayerController pc;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        itemChanged = false;
    }

    public void AddItem(ShopItem item) {
        shopItems.Add(item);
    }

    public void RemoveItem(ShopItem item) {
        pc = FindObjectOfType<PlayerController>();
        pc.AddEffect(item.ItemID);
        shopItems.Remove(item);
        GameManager.instance.Storage--;
        itemChanged = true;
    }
}
