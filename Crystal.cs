using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private GameObject collides;
    private int maxHealth = 5000;
    private int currHealth;

    public void Start() {
        healthBar.setMax(maxHealth);
        currHealth = maxHealth;
    }

    public void Update() {
        healthBar.setHealth(currHealth);
    }

    public void TakeDamage(int amount) {
        currHealth -= amount;

        if (currHealth <= 0) {
            if (gameObject != null) {
                Destroy(gameObject);
                GameObject explode = Instantiate(collides, transform.position, transform.rotation);
                Destroy(explode, 0.5f);
            }
            GameManager.instance.EndGame(1);
        }
    }
}
