using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : ShopItem
{
    private int healthHeal = 150;

    public int HealthHeal {
        get { return healthHeal; }
    }
}
