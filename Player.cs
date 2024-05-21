using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  

public abstract class Player : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed, coef;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;
    [SerializeField] private bool canJump;
    [SerializeField] protected AudioSource attackSfx;
    [SerializeField] protected int playerHealth;
    [SerializeField] protected int playerMana;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected ManaBar manaBar;
    [SerializeField] protected HealthBar healthBarFloating;
    protected int currHealth;
    protected int currMana;
    public Canvas healthBarFloatingCanvas;
    public float jumpForce = 15f;
    public bool isOnGround = true;
    protected bool activeCharacter = false;
    protected float rotationSpeed = 720f;
    protected int specialManaCost = 5;
    protected bool isJumping = false;
    protected Animator animator;
    protected int isWalkHash;
    protected int isRunHash;
    protected int isJumpHash;
    protected int isBasicHash;
    protected int isHeavyHash;
    protected int isSpecialHash;
    protected Vector3 moveDirection;
    protected bool isAttack;
    protected abstract void OverrideStart();
    protected abstract void OverrideUpdate();
    protected abstract void OverrideFixedUpdate();
    protected abstract IEnumerator attackFalse(float duration); 
    protected abstract IEnumerator isBasicFalse(float duration);
    protected abstract IEnumerator isHeavyFalse(float duration);
    protected abstract IEnumerator isSpecialFalse(float duration);
    PlayerController pc;
    private EnemyController ec;
    private Grid grid;
    private Pathfinding pf;
    private Transform target;
    private Vector3 beforePosition;
    private Vector3[] path;
    private int targetIndex;
    private float beforeTime = 0f;
    protected bool botAttacking = false;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isWalkHash = Animator.StringToHash("isWalk");
        isRunHash = Animator.StringToHash("isRunning");
        isBasicHash = Animator.StringToHash("isBasic");
        isHeavyHash = Animator.StringToHash("isHeavy");
        isSpecialHash = Animator.StringToHash("isSpecial");
        if(canJump) {
            isJumpHash = Animator.StringToHash("isJump");
        }
        currHealth = playerHealth;
        currMana = playerMana;
        healthBarFloating.setMax(playerHealth);
        healthBar.setMax(playerHealth);
        manaBar.setMax(playerMana);
        isAttack = false;

        ec = FindObjectOfType<EnemyController>();
        grid = FindObjectOfType<Grid>();
        pf = FindObjectOfType<Pathfinding>();

        OverrideStart();
    }

    public void FixedUpdate() {
        if(activeCharacter) {
            Movement();
        }
        OverrideFixedUpdate();
    }

    public void Update() {
        healthBarFloating.setHealth(currHealth);
        healthBar.setHealth(currHealth);
        manaBar.setMana(currMana);
        SurfaceAlignment();

        if(activeCharacter) {
            CancelPath();
            botAttacking = false;
            AnimationControl();
            if(canJump) {
                if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
                    Jump();
                }
            }
        }

        if(!activeCharacter) {
            if(ec.enemyList.Count > 0) {
                ClosestEnemy();
                
                if(currHealth > 20) {
                    if((Time.time - beforeTime) > 0.6f) {
                        if(target.name == "Polyart_Golem") {
                            if(beforePosition != target.position) {
                                if(Vector3.Distance(transform.position, target.position) > 0.8f) {
                                    CancelPath();
                                    botAttacking = false;
                                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                                } 
                                beforePosition = target.position;
                            }
                        } else {
                            if(this.gameObject.name == "Patrick" || this.gameObject.name == "Tim") {
                                if(this.gameObject.name == "Tim" && target.name == "GruntPolyart(Clone)") {
                                    if(Vector3.Distance(transform.position, target.position) > 0.45f) {
                                        CancelPath();
                                        botAttacking = false;
                                        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                                    } 
                                } else {
                                    if(Vector3.Distance(transform.position, target.position) > 0.6f) {
                                        CancelPath();
                                        botAttacking = false;
                                        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                                    } 
                                }
                            } else {    
                                if(Vector3.Distance(transform.position, target.position) > 0.8f) {
                                    CancelPath();
                                    botAttacking = false;
                                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                                } 
                            }
                        }
                        beforePosition = target.position;
                        beforeTime = Time.time + 0.6f;
                    }
                }

                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 300f * Time.deltaTime);
            }
        }

            if(isAttack && activeCharacter) {
                moveDirection = Camera.main.transform.forward;
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                Quaternion smoother = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotation.eulerAngles.y, transform.eulerAngles.z);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

        OverrideUpdate();
    }

    private void ClosestEnemy() {
        float closest = Mathf.Infinity;

        foreach (GameObject e in ec.GetEnemies()) {
            int distance = pf.getDistance(grid.NodeFromWorldPoint(e.transform.position), grid.NodeFromWorldPoint(transform.position));
            if (distance < closest){
                closest = distance;
                target = e.transform;
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

    public void CancelPath() {
        if(path != null) {
            StopCoroutine("FollowPath");
            animator.SetBool(isWalkHash, false);
        }
    }

    IEnumerator FollowPath() {
        float thresholdDistance = 0.75f;
        if(this.gameObject.name == "Patrick") {
            if(target.name == "GruntPolyart(Clone)") {
                thresholdDistance = 0.45f;
            } else {
                thresholdDistance = 0.85f;
            }
        } else if(this.gameObject.name == "Tim") {
            if(target.name == "GruntPolyart(Clone)") {
                thresholdDistance = 0.35f;
            }
        }
        animator.SetBool(isWalkHash, true);

        if(path.Length > 0) {
            Vector3 currWaypoint = path[0];
            Debug.Log("CURR WAY POINT = " + currWaypoint);
            Debug.Log("Array pathnya = " + path);
            Debug.Log(this.gameObject.name + " = " + Vector3.Distance(transform.position, target.position) + " -> " + target.gameObject.name);
            Debug.Log(Vector3.Distance(transform.position, target.position) + " > " + thresholdDistance + " = " + (Vector3.Distance(transform.position, target.position) > thresholdDistance));
            if (Vector3.Distance(transform.position, target.position) > thresholdDistance) {
                if(gameObject != null) {
                    while (true) {
                        float distanceToWaypoint = Vector3.Distance(transform.position, currWaypoint);
                        
                        // Debug.Log("DISTANCE WAYPOINT = " + distanceToWaypoint);
                        if (distanceToWaypoint <= thresholdDistance) {
                            targetIndex++;
                            Debug.Log("TARGET INDEX = "+ targetIndex);
                            Debug.Log("PATH LENGTH = "+ path.Length);
                            Debug.Log("target index path length = " + (targetIndex >= path.Length));
                            if (targetIndex >= path.Length) {
                                animator.SetBool(isWalkHash, false);
                                botAttacking = true;
                                // AttackPlayer(); 
                                yield break;
                            }
                            currWaypoint = path[targetIndex];
                        }

                        transform.position = Vector3.MoveTowards(transform.position, currWaypoint, 2 * Time.deltaTime);
                        yield return null;

                        
                    }
                } else {
                    yield return null;
                }
            } else {
                Debug.Log("threshold = " + thresholdDistance);
            }
        } else {
            animator.SetBool(isWalkHash, false);
            botAttacking = true;
            Debug.Log("PATH LENGTHNYA DAH 0");
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

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
    }

    private void OnCollisionEnter(Collision col) {
        if(canJump) {
            if (col.gameObject.CompareTag("Ground")) {
                isJumping = false;
                animator.SetBool(isJumpHash, false);
            }
        }
    }

    public void Movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 frontBack = Camera.main.transform.forward;
        frontBack.y = 0f;
        frontBack.Normalize();

        Vector3 leftRight = Camera.main.transform.right;
        leftRight.y = 0f;
        leftRight.Normalize();

        Vector3 moveDirection = frontBack * verticalInput + leftRight * horizontalInput;
        moveDirection.Normalize();

        Vector3 counterMovement = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);

        float currSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            currSpeed = speed * 3f;
        }

        rb.AddForce(moveDirection * currSpeed);
        rb.AddForce(counterMovement * coef);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion smoother = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotation.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }

    private void SurfaceAlignment() {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit infoRayHit = new RaycastHit();
        Quaternion rotationRef = Quaternion.Euler(0, 0, 0);

        if(Physics.Raycast(ray, out infoRayHit, ground)) {
            if(Input.GetKey(KeyCode.S)) {
                rotationRef = Quaternion.Euler(-rotationRef.eulerAngles.x, rotationRef.eulerAngles.y, rotationRef.eulerAngles.z);
            }
            
            Quaternion newRotate = Quaternion.Euler(rotationRef.eulerAngles.x, transform.eulerAngles.y, rotationRef.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotate, curve.Evaluate(time));
        }
    }

    private void AnimationControl() {
        bool isWalk = animator.GetBool(isWalkHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isJump;
        if(canJump) {
            isJump = animator.GetBool(isJumpHash);
        } else {
            isJump = false;
        }
        bool walkingForward = Input.GetKey(KeyCode.W);
        bool walkingLeft = Input.GetKey(KeyCode.A);
        bool walkingBackward = Input.GetKey(KeyCode.S);
        bool walkingRight = Input.GetKey(KeyCode.D);
        bool running = Input.GetKey(KeyCode.LeftShift);
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        if(activeCharacter) { 
            if (!isWalk && (walkingForward || walkingLeft || walkingBackward || walkingRight)) {
                animator.SetBool(isWalkHash, true);
            }

            if (isWalk && !(walkingForward || walkingLeft || walkingBackward || walkingRight)) {
                animator.SetBool(isWalkHash, false);
            }

            if (!isRun && running && (walkingForward || walkingLeft || walkingBackward || walkingRight)) {           
                animator.SetBool(isRunHash, true);
            }

            if (isRun && (!running || !(walkingForward || walkingLeft || walkingBackward || walkingRight))) {
                animator.SetBool(isRunHash, false);
            }

            if(canJump) {
                if (!isJump && jumping) {
                    animator.SetBool(isJumpHash, true);
                }
            } 
        }
    }

    public void SetActiveCharacter(bool isActive) {
        activeCharacter = isActive;
    }

    public void StopAnimation() {
        animator.SetBool(isWalkHash, false);
        animator.SetBool(isRunHash, false);
        animator.SetBool(isBasicHash, false);
        animator.SetBool(isHeavyHash, false);
        animator.SetBool(isSpecialHash, false);
        if(canJump) {
            animator.SetBool(isJumpHash, false);
        }
    }

    public void TakeDamage(int amount) {
        currHealth -= amount;

        if (currHealth <= 0) {
            pc = FindObjectOfType<PlayerController>();
            pc.PlayerDie(this);
        }
    }

    public void AddManaPotion(ManaPotion mp) {
        currMana += mp.ManaRestoration;
        if(currMana > playerMana) {
            currMana = playerMana;
        }
    }

    public void AddHealthPotion(HealthPotion hp) {
        currHealth += hp.HealthHeal;
        if(currHealth > playerHealth) {
            currHealth = playerHealth;
        }
    }

    public void AddHybridPotion(HybridPotion hp) {
        currHealth += hp.HealthHeal;
        currMana += hp.ManaRestoration;

        if(currHealth > playerHealth) {
            currHealth = playerHealth;
        }

        if(currMana > playerMana) {
            currMana = playerMana;
        }
    }
}
