using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridPotion : ShopItem
{
    private int manaRestoration = 50;
    private int healthHeal = 150;

    public int ManaRestoration {
        get { return manaRestoration; }
    }

    public int HealthHeal {
        get { return healthHeal; }
    }
}
