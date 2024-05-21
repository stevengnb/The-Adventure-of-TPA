using UnityEngine;
using Cinemachine;

public class NpcInteract : MonoBehaviour
{

    private bool interacting = false;
    private bool inRange = false;
    public GameObject Menu;
    public GameObject ShowInteract;
    public GameObject funds;
    public GameObject inventory;
    private PlayerMovement playerMovement;
    private LockCameraPosition cameraController;
    public CinemachineFreeLook normal;
    public CinemachineVirtualCamera npc;
    private SwordInteraction swordInteraction;

    private void Awake() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraController = FindObjectOfType<LockCameraPosition>();
        GameObject swordObject = GameObject.Find("Sword");
        swordInteraction = swordObject.GetComponent<SwordInteraction>();
        npc.Priority = 0;
    }

    private void Update() {
        if (!(swordInteraction.Interacting)) {
            if(inRange && Input.GetKeyUp(KeyCode.F)) {
                if (!interacting) {
                    normal.Priority = 0;
                    npc.Priority = 10;
                    playerMovement.FreezePlayer();
                    interacting = true;
                    Menu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                } else if (Input.GetKeyUp(KeyCode.F) && interacting) {
                    normal.Priority = 10;
                    npc.Priority = 0;
                    playerMovement.UnfreezePlayer();
                    interacting = false;
                    Menu.SetActive(false);
                    funds.SetActive(false);
                    inventory.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
    
    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            if(!interacting){
                ShowInteract.SetActive(true);
            } else {
                ShowInteract.SetActive(false);
            }
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        ShowInteract.SetActive(false);
        inRange = false;
    }
}
