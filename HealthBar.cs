using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider inside;
    public Gradient gradient;
    public Image fill;
    public void setMax(int health) {
        inside.maxValue = health;
        inside.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth(int health) {
        inside.value = health;
        fill.color = gradient.Evaluate(inside.normalizedValue);
    }
}
