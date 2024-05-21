using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDogKnight : Player
{
    private SwordTwo sword;
    [SerializeField] protected AudioSource swordSfx;
    [SerializeField] protected AudioSource swordSfxTwo;

    protected override void OverrideStart() {
        sword = FindObjectOfType<SwordTwo>();
        isAttack = false;
    }

    protected override void OverrideFixedUpdate() {

    }

    protected override void OverrideUpdate() {
        attack();

    }

    private void attack() {
        bool isWalk = animator.GetBool(isWalkHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isJump = animator.GetBool(isJumpHash);
        bool isBasic = animator.GetBool(isBasicHash);
        bool isHeavy = animator.GetBool(isHeavyHash);
        bool isSpecial = animator.GetBool(isSpecialHash);
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        bool specialClick = Input.GetKeyUp(KeyCode.X);

        if(activeCharacter) {
            if(!isBasic && leftClick) {
                swordSfx.Play();
                animator.SetBool(isBasicHash, true);
                sword.BasicAttack();
                StartCoroutine(isBasicFalse(0.5f));
            } 

            if(!isHeavy && rightClick) {
                swordSfxTwo.Play();
                animator.SetBool(isHeavyHash, true);
                sword.HeavyAttack();
                StartCoroutine(isHeavyFalse(0.5f));
            }

            if(!isSpecial && specialClick && (!isWalk && !isRun && !isJump && !isBasic && !isHeavy)) {
                if(currMana >= 5) {
                    isAttack = true;
                    animator.SetBool(isSpecialHash, true);
                    StartCoroutine(attackSound(0.75f));
                    StartCoroutine(attackFalse(0.5f));
                    StartCoroutine(isSpecialFalse(2f));
                    currMana -= specialManaCost;
                }
            }
        }

        if(!activeCharacter) {
            if(botAttacking) {
                if(!isBasic && botAttacking) {
                    animator.SetBool(isBasicHash, true);
                    sword.BasicAttack();
                    StartCoroutine(isBasicFalse(1f));
                }
            }
        }
    }

    protected override IEnumerator attackFalse(float duration) {
        yield return new WaitForSeconds(duration);
        isAttack = false;
    }

    protected override IEnumerator isBasicFalse(float duration) {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isBasicHash, false);
    }

    protected override IEnumerator isHeavyFalse(float duration) {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isHeavyHash, false);
    }

    protected override IEnumerator isSpecialFalse(float duration) {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isSpecialHash, false);
    }

    private IEnumerator attackSound(float duration) {
        yield return new WaitForSeconds(duration);
        attackSfx.Play();
        sword.specialProjectile();
    }
}
