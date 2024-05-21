using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed, coef;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;
    private float rotationSpeed = 720f;
    public float jumpForce = 15f;
    public bool isOnGround = true;
    private bool isJumping = false;
    private bool isFrozen = false;

    public void Start() {
        rb = GetComponent<Rigidbody>();   
    }

    public void FixedUpdate() {
        if (!isFrozen) {
            Movement();
        }
    }
    
    public void Update() {
        SurfaceAlignment();
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            Jump();
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            isJumping = false;
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

    public void FreezePlayer() {
        isFrozen = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezePlayer() {
        isFrozen = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.constraints &= ~RigidbodyConstraints.FreezePosition;
    }

    public bool IsFrozen() {
        return isFrozen;
    }
}
