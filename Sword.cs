using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform specialSpawn;
    public GameObject specialPrObject;
    public float projectileSpeed = 10f;
    private bool isBasicAttack;
    private bool isHeavyAttack;

    public void specialProjectile() {
        var projectile = Instantiate(specialPrObject, specialSpawn.position, specialSpawn.rotation);
        attackNow(projectile);
    }

    private void attackNow(GameObject projectile) {
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
        Vector3 cameraMiddleDirection = mainCamera.ViewportPointToRay(screenCenter).direction;

        projectile.GetComponent<Rigidbody>().velocity = cameraMiddleDirection * projectileSpeed;
    }

    public void BasicAttack() {
        isBasicAttack = true;
        isHeavyAttack = false;
        StartCoroutine(BAFalse(0.5f));
    }

    public void HeavyAttack() {
        isBasicAttack = false;
        isHeavyAttack = true;
        StartCoroutine(HAFalse(1.25f));
    }

    private IEnumerator BAFalse(float duration) {
        yield return new WaitForSeconds(duration);
        isBasicAttack = false;
    }

    private IEnumerator HAFalse(float duration) {
        yield return new WaitForSeconds(duration);
        isHeavyAttack = false;
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if (isBasicAttack){
                    enemy.TakeDamage(15);
                    isBasicAttack = false;
                } else if (isHeavyAttack) {
                    enemy.TakeDamage(25);
                    isHeavyAttack = false;
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if (isBasicAttack){
                    enemy.TakeDamage(15);
                    isBasicAttack = false;
                } else if (isHeavyAttack) {
                    enemy.TakeDamage(25);
                    isHeavyAttack = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if (isBasicAttack){
                    enemy.TakeDamage(15);
                    isBasicAttack = false;
                } else if (isHeavyAttack) {
                    enemy.TakeDamage(25);
                    isHeavyAttack = false;
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if (isBasicAttack){
                    enemy.TakeDamage(15);
                    isBasicAttack = false;
                } else if (isHeavyAttack) {
                    enemy.TakeDamage(25);
                    isHeavyAttack = false;
                }
            }
        }
    }
}
