using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider inside;
    public void setMax(int health) {
        inside.maxValue = health;
        inside.value = health;
    }

    public void setMana(int health) {
        inside.value = health;
    }
}
