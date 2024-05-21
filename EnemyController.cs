using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyTower;
    public GameObject enemyPlayer;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public float spawnInterval;
    public int maxEnemy;
    private int activeEnemy;
    public List<GameObject> enemyList = new List<GameObject>();
    private GameObject newEnemy;

    private void Start() {
        GameManager.instance.enemyKilled = 0;
        activeEnemy = 0;
        EnemyAtStart();
    }

    private void EnemyAtStart() {
        newEnemy = Instantiate(enemyTower, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation);
        enemyList.Add(newEnemy);

        newEnemy = Instantiate(enemyTower, spawnPoints[5].transform.position, spawnPoints[5].transform.rotation);
        enemyList.Add(newEnemy);

        int rand = Random.Range(0, spawnPoints.Count);
        newEnemy = Instantiate(enemyPlayer, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
        enemyList.Add(newEnemy);
        
        activeEnemy += 3;

        InvokeRepeating ("SpawnEnemy", 10f, spawnInterval);
    }

    public List<GameObject> GetEnemies() {
        return enemyList;
    }

    private void SpawnEnemy() {
        if (activeEnemy >= maxEnemy) {
            return;
        }

        int rand = Random.Range(0, spawnPoints.Count);
        int randEnemy = Random.Range(0, 10);
        if(randEnemy > 3) {
            newEnemy = Instantiate(enemyTower, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
        } else {
            newEnemy = Instantiate(enemyPlayer, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
        }

        enemyList.Add(newEnemy);
        activeEnemy++;
    }

    public void RemoveEnemy(GameObject enemy) {
        enemyList.Remove(enemy);
    }

    public void EnemyDied() {
        activeEnemy--;
        GameManager.instance.enemyKilled++;

        if (activeEnemy < maxEnemy) {
            Invoke("SpawnEnemy", spawnInterval);
        }
    }

}
