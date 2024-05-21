using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialProjectile : MonoBehaviour
{
    [SerializeField] private GameObject collides;
    public float duration = 10f;
    public Vector3 targetScale = new Vector3(1.25f, 1.25f, 1.25f);
    public float scaleDuration = 1.5f;
    private Vector3 initialScale;
    private bool isScaling = false;

    private void Awake() {
        Destroy(gameObject, duration);
        StartScaling();
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Enemy") {
            EnemyTower enemy = col.gameObject.GetComponent<EnemyTower>();

            if (enemy != null) {
                if(gameObject.tag == "Wizs") {
                    enemy.TakeDamage(40);
                }
            }
        } else if(col.gameObject.tag == "EnemyP") {
            EnemyPlayer enemy = col.gameObject.GetComponent<EnemyPlayer>();

            if (enemy != null) {
                if(gameObject.tag == "Wizs") {
                    enemy.TakeDamage(40);
                }
            }
        }

        Destroy(gameObject);
        GameObject explopde = Instantiate(collides, transform.position, transform.rotation);
        Destroy(explopde, 0.5f);
    }

    public void StartScaling() {
        if (!isScaling) {
            initialScale = transform.localScale;
            StartCoroutine(ScaleProjectile());
        }
    }

    private IEnumerator ScaleProjectile() {
        isScaling = true;
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration) {
            float t = elapsedTime / scaleDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        isScaling = false;
    }
}
