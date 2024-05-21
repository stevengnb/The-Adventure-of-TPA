using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHand : MonoBehaviour
{
    private bool isAttacking;

    private void Start() {
        isAttacking = false;
    }

    public void notAttack() {
        isAttacking = false;
    }

    public void isAttack() {
        isAttacking = true;
    }

    private void OnTriggerEnter(Collider col) {
        if(isAttacking) {
            if(col.gameObject.tag == "Crystal") {
                Crystal crystal = col.gameObject.GetComponent<Crystal>();

                if (crystal != null) {
                    crystal.TakeDamage(20);
                }
            } else if(col.gameObject.tag == "Player") {
                Player player = col.gameObject.GetComponent<Player>();

                if(player != null) {
                    player.TakeDamage(20);
                }
            }
        }
    }
}
