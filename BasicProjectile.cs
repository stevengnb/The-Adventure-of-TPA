using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{   
    [SerializeField] private GameObject collides;
    private float duration = 10f;

    private void Awake() {
        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if (gameObject.tag == "Tims"){
                    enemy.TakeDamage(40);
                } else if (gameObject.tag == "Patricks") {
                    enemy.TakeDamage(45);
                } else if(gameObject.tag == "Wizs") {
                    enemy.TakeDamage(20);
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if (gameObject.tag == "Tims"){
                    enemy.TakeDamage(40);
                } else if (gameObject.tag == "Patricks") {
                    enemy.TakeDamage(45);
                } else if(gameObject.tag == "Wizs") {
                    enemy.TakeDamage(20);
                }
            }
        }
        
        Destroy(gameObject);
        GameObject explopde = Instantiate(collides, transform.position, transform.rotation);
        Destroy(explopde, 0.5f);
    }
}
