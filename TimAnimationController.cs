using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimAnimationController : MonoBehaviour
{

    public Animator animator;
    private int isWalkHash;
    private int isRunHash;
    private int isJumpHash;
    private int isChangeHash;
    private PlayerMovement playerMovement;

    public void Start() {
        animator = GetComponent<Animator>();
        isWalkHash = Animator.StringToHash("isWalk");
        isRunHash = Animator.StringToHash("isRunning");
        isJumpHash = Animator.StringToHash("isJump");
        isChangeHash = Animator.StringToHash("isChange");
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Update() {
        bool isWalk = animator.GetBool(isWalkHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isJump = animator.GetBool(isJumpHash);
        bool walkingForward = Input.GetKey(KeyCode.W);
        bool walkingLeft = Input.GetKey(KeyCode.A);
        bool walkingBackward = Input.GetKey(KeyCode.S);
        bool walkingRight = Input.GetKey(KeyCode.D);
        bool running = Input.GetKey(KeyCode.LeftShift);
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        bool isFrozen = playerMovement.IsFrozen();

        if (isFrozen) {
            animator.SetBool(isWalkHash, false);
            animator.SetBool(isRunHash, false);
            animator.SetBool(isJumpHash, false);
            return;
        }

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

        if (!isJump && jumping) {
            animator.SetBool(isJumpHash, true);
        }
    }

    public void ChangeScene() {
        animator.SetBool(isChangeHash, true);
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            animator.SetBool(isJumpHash, false);
        }
    }
}
