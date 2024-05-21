using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : MonoBehaviour

{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private GameObject collides;
    [SerializeField] private GameObject blood;
    private EnemyController ec;
    private int maxHealth = 300;
    private int currHealth;
    private EnemyTowerAxe axe;
    public Transform target;
    private Vector3[] path;
    private int targetIndex;
    private bool isFollowing;
    private bool isRequestingPath;
    protected Animator animator;
    protected int isWalkHash;
    protected int isAttackHash;

    public void Awake() {
        ec = FindObjectOfType<EnemyController>();
        target = GameObject.FindGameObjectWithTag("CrystalTarget").transform;
    }

    public void Start() {
        axe = FindObjectOfType<EnemyTowerAxe>(); 
        healthBar.setMax(maxHealth);
        currHealth = maxHealth;
        isFollowing = false;
        isRequestingPath = false;
        animator = GetComponent<Animator>();
        isWalkHash = Animator.StringToHash("isWalk");
        isAttackHash = Animator.StringToHash("isAttack");
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void Update() {
        healthBar.setHealth(currHealth);
        transform.LookAt(target);

        if (isFollowing) {
            if (path != null && targetIndex < path.Length) {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 300f * Time.deltaTime);
            }
        }

        if(currHealth > 50) {
            if(!isFollowing && !isRequestingPath) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                float areaThreshold = 5.5f;

                if (distanceToTarget > areaThreshold) {
                    isRequestingPath = true;
                    Debug.Log("Requesting path");
                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                }
            }
        }
    }

    public void TakeDamage(int amount) {
        currHealth -= amount;
        GameObject bloodGO = Instantiate(blood, transform.position + new Vector3(0, 1, 0), transform.rotation);
        Destroy(bloodGO, 1.25f);

        if (currHealth <= 0) {
            EnemyTowerDie();
        }
    }

    

    private void EnemyTowerDie() {
        if (gameObject != null) {
            ec.RemoveEnemy(this.gameObject);
            ec.EnemyDied();
            Destroy(gameObject);
            GameObject explode = Instantiate(collides, transform.position, transform.rotation);
            Destroy(explode, 0.5f);
        }
    }
    
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        isRequestingPath = false;

        Debug.Log(gameObject);
        if(gameObject != null) {
            if(pathSuccessful) {
                isFollowing = true;
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
    }   

    private IEnumerator FollowPath() {
        const float thresholdDistance = 3.5f;
        axe.notAttack();
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isWalkHash, true);

        Vector3 currWaypoint = path[0];
        
        if(gameObject != null) {
            while (true) {
                float distanceToWaypoint = Vector3.Distance(transform.position, currWaypoint);
                
                if (distanceToWaypoint <= thresholdDistance) {
                    targetIndex++;
                    if (targetIndex >= path.Length) {
                        targetIndex = 0;
                        path = new Vector3[0];
                        isFollowing = false;
                        animator.SetBool(isWalkHash, false);
                        animator.SetBool(isAttackHash, true);
                        axe.isAttack();
                        yield break;
                    }
                    currWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currWaypoint, 1.5f * Time.deltaTime);
                yield return null;
            }
        } else {
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if(path != null) {
            for(int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
