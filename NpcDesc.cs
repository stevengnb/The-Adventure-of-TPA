using UnityEngine;

public class NpcDesc : MonoBehaviour
{
    public Transform Cam;

    private void Start() {
        Cam = Camera.main.transform;
    }

    private void Update() {
        transform.rotation = Quaternion.LookRotation(transform.position - Cam.transform.position);
    }
}
