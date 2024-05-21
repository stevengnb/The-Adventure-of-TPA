using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerWizard : Player
{
    private Stick stick;
    public CinemachineFreeLook normal;
    public CinemachineVirtualCamera pov;
    private bool isPOV = false;

    protected override void OverrideStart() {
        stick = FindObjectOfType<Stick>();
        isAttack = false;
    }

    protected override void OverrideFixedUpdate() {

    }

    protected override void OverrideUpdate() {
        attack();
        if(isAttack) {
            moveDirection = Camera.main.transform.forward;
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion smoother = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotation.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void attack() {
        bool isWalk = animator.GetBool(isWalkHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isBasic = animator.GetBool(isBasicHash);
        bool isSpecial = animator.GetBool(isSpecialHash);
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        bool specialClick = Input.GetKeyUp(KeyCode.X);

        if(activeCharacter) {
            if(!isBasic && leftClick) {
                isAttack = true;
                animator.SetBool(isBasicHash, true);
                StartCoroutine(attackFalse(0.5f));
                StartCoroutine(setBasicProjectile(0.5f));
                StartCoroutine(isBasicFalse(1f));
            } 

            if (isPOV && rightClick) {
                isPOV = false;
                normal.Priority = 10;
                pov.Priority = 9;
            } else if (!isPOV && rightClick) {
                isPOV = true;
                normal.Priority = 9;
                pov.Priority = 10;
            }

            if(!isSpecial && specialClick && (!isWalk && !isRun && !isBasic)) {
                if(currMana >= 5) {
                    isAttack = true;
                    attackSfx.Play();
                    animator.SetBool(isSpecialHash, true);
                    Invoke("setSpecial", 0.75f);
                    StartCoroutine(attackFalse(0.5f));
                    StartCoroutine(isSpecialFalse(2.3f));
                    currMana -= specialManaCost;
                }
            }
        }

        if(botAttacking) {
                if(!isBasic && botAttacking) {
                    BotWizardAttack();
                }
            }
    }
    protected void BotWizardAttack() {
        animator.SetBool(isBasicHash, true);
        stick.BotAttack();
        StartCoroutine(isBasicFalse(1f));
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
    }


    protected override IEnumerator isSpecialFalse(float duration) {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isSpecialHash, false);
    }

    private IEnumerator setBasicProjectile(float duration) {
        yield return new WaitForSeconds(duration);
        stick.basicProjectile();
    }

    private void setSpecial() {
        stick.specialProjectile();
    }
}
