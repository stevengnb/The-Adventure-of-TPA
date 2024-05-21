using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public Transform projectileSpawn;
    public Transform specialSpawn;
    public GameObject basicPrObject;
    public GameObject specialPrObject;
    public float projectileSpeed = 10f;
    private GameObject currentSpecialProjectile;
    private bool isBotAttack;

    public void basicProjectile() {
        var projectile = Instantiate(basicPrObject, projectileSpawn.position, projectileSpawn.rotation);
        attackNow(projectile);
    }

    public void BotAttack() {
        isBotAttack = true;
        StartCoroutine(BAFalse(0.5f));
    }

    private IEnumerator BAFalse(float duration) {
        yield return new WaitForSeconds(duration);
        isBotAttack = false;
    }

    public void specialProjectile() {
        currentSpecialProjectile = Instantiate(specialPrObject, specialSpawn.position, specialSpawn.rotation);
        StartCoroutine(waitScale(3f));
    }
    
    private void attackNow(GameObject projectile) {
        Camera mainCamera = Camera.main;
        Debug.Log(mainCamera);
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
        Vector3 cameraMiddleDirection = mainCamera.ViewportPointToRay(screenCenter).direction;

        projectile.GetComponent<Rigidbody>().velocity = cameraMiddleDirection * projectileSpeed;
    }

    private IEnumerator waitScale(float duration) {
        yield return new WaitForSeconds(duration);
        if (currentSpecialProjectile != null) {
            attackNow(currentSpecialProjectile);
            currentSpecialProjectile = null;
        }
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if (isBotAttack){
                    enemy.TakeDamage(20);
                    isBotAttack = false;
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if (isBotAttack){
                    enemy.TakeDamage(20);
                    isBotAttack = false;
                }
            }
        }
    }
    private void OnTriggerStay(Collider col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if (isBotAttack){
                    enemy.TakeDamage(20);
                    isBotAttack = false;
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if (isBotAttack){
                    enemy.TakeDamage(20);
                    isBotAttack = false;
                }
            }
        }
    }
    
}
