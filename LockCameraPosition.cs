using UnityEngine;
using Cinemachine;

public class LockCameraPosition : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    public void LockCamera() {
        this.gameObject.SetActive(false);
    }

    public void UnlockCamera() {
        this.gameObject.SetActive(true);
    }
}
