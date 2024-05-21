using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : ShopItem
{
    private int manaRestoration = 50;

    public int ManaRestoration {
        get { return manaRestoration; }
    }
}
