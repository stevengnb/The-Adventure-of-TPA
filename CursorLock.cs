using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private void Start() {
       Lock();
    }

    private void Lock(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
