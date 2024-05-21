using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private GameObject collides;
    [SerializeField] private GameObject blood;
    private EnemyController ec;
    private int maxHealth = 300;
    private int currHealth;
    private EnemyPlayerHand hand;
    private Vector3[] path;
    private Transform target;
    private Vector3 beforePosition;
    private int targetIndex;
    private PlayerController players;
    private Grid grid;
    private Pathfinding pf;
    protected Animator animator;
    protected int isWalkHash;
    protected int isAttackHash;
    private float beforeTime = 0f;

    public void Awake() {
        ec = FindObjectOfType<EnemyController>();
    }

    private void Start() {
        hand = FindObjectOfType<EnemyPlayerHand>();
        players = FindObjectOfType<PlayerController>();
        grid = FindObjectOfType<Grid>();
        pf = FindObjectOfType<Pathfinding>();
        animator = GetComponent<Animator>();
        healthBar.setMax(maxHealth);
        currHealth = maxHealth;
        isWalkHash = Animator.StringToHash("isWalk");
        isAttackHash = Animator.StringToHash("isAttack");
    }

    private void Update() {
        ClosestPlayer();
        healthBar.setHealth(currHealth);
        transform.LookAt(target);
        
        if(currHealth > 25) {
            if((beforePosition != target.position) && ((Time.time - beforeTime) > 0.6f)){
                if(Vector3.Distance(transform.position, target.position) > 1.35f) {
                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                }
                beforePosition = target.position;
                beforeTime = Time.time + 0.6f;
            }
        }

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 300f * Time.deltaTime);
    }

    private void ClosestPlayer() {
        float closest = Mathf.Infinity;

        foreach (Player p in players.GetPlayers()) {
            int distance = pf.getDistance(grid.NodeFromWorldPoint(p.gameObject.transform.position), grid.NodeFromWorldPoint(transform.position));
            if (distance < closest){
                closest = distance;
                target = p.gameObject.transform;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if(gameObject != null) {
            if(pathSuccessful) {
                targetIndex = 0;
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator FollowPath() {
        float thresholdDistance = 1.35f;
        if(target.name == "Patrick") {
            thresholdDistance = 1.5f;
        }

        animator.SetBool(isWalkHash, true);
        animator.SetBool(isAttackHash, false);
        hand.notAttack();

        Vector3 currWaypoint;
        if (path.Length > 0 && Vector3.Distance(transform.position, target.position) > thresholdDistance) {
            currWaypoint = path[0];
            if(gameObject != null) {
                while (true) {
                    float distanceToWaypoint = Vector3.Distance(transform.position, currWaypoint);
                    
                    // Debug.Log(distanceToWaypoint);
                    if (distanceToWaypoint <= thresholdDistance) {
                        targetIndex++;
                        if (targetIndex >= path.Length) {
                            animator.SetBool(isWalkHash, false);
                            AttackPlayer(); 
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
        } else {
            animator.SetBool(isAttackHash, true);
        }
    }

    private void AttackPlayer() {
        animator.SetBool(isAttackHash, true);
        hand.isAttack();
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

    public void TakeDamage(int amount) {
        currHealth -= amount;
        GameObject bloodGO = Instantiate(blood, transform.position + new Vector3(0, 1, 0), transform.rotation);
        Destroy(bloodGO, 1.25f);

        if (currHealth <= 0) {
            EnemyPlayerDie();
        }
    }

    private void EnemyPlayerDie() {
        if (gameObject != null) {
            StopCoroutine("FollowPath");
            ec.RemoveEnemy(this.gameObject);
            ec.EnemyDied();
            Destroy(gameObject);
            GameObject explode = Instantiate(collides, transform.position, transform.rotation);
            Destroy(explode, 0.5f);
        }
    }
}