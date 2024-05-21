using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject collides;
    public List<Player> players;
    public List<CinemachineFreeLook> cameras;
    public CinemachineVirtualCamera wizards;
    public List<GameObject> bar;
    public GameObject inventoryMenu;
    private bool interacting = false;
    private int activeIdx = 0;
    private int newActiveIdx = 0;
    private bool switching = false;
    private HealthPotion hp;
    private ManaPotion mp;
    private HybridPotion hybrid;

    private void Awake()
    {
        SetActivePlayer(activeIdx);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start() {
        TimeController.instance.startTime();
        hp = FindObjectOfType<HealthPotion>();
        Debug.Log(hp);
        mp = FindObjectOfType<ManaPotion>();
        hybrid = FindObjectOfType<HybridPotion>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q) && !switching) {
            players[activeIdx].StopAnimation();
            if (activeIdx < players.Count - 1) {
                newActiveIdx = activeIdx + 1;
            } else {
                newActiveIdx = 0;
            }
            StartCoroutine(SwitchCameraCoroutine(newActiveIdx));
        }

        if(Input.GetKeyUp(KeyCode.I) && !switching) {
            if (!interacting) {
                interacting = true;
                inventoryMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else if (Input.GetKeyUp(KeyCode.I) && interacting) {
                interacting = false;
                inventoryMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
        }
    }

    public List<Player> GetPlayers() {
        return players;
    }

    public void AddEffect(int id) {
        if(id == 0) {
            players[newActiveIdx].AddManaPotion(mp);
        } else if(id == 1) {
            players[newActiveIdx].AddHealthPotion(hp);
        } else if(id == 2) {
            players[newActiveIdx].AddHybridPotion(hybrid);
        }
    }

    private void SetActivePlayer(int idx) {
        wizards.Priority = 0;
        for (int i = 0; i < players.Count; i++) {
            if (i == idx) {
                players[i].SetActiveCharacter(true);
                cameras[i].Priority = 10;
                bar[i].SetActive(true);
            } else {
                players[i].SetActiveCharacter(false);
                cameras[i].Priority = 0;
                bar[i].SetActive(false);
            }
        }
    }

    private IEnumerator SwitchCameraCoroutine(int newActiveIdx) {
        switching = true;

        if(players[activeIdx].name == "Araszkiewicz") {
            wizards.Priority = 0;
        }
        cameras[activeIdx].Priority = 0;
        players[activeIdx].SetActiveCharacter(false);
        players[activeIdx].healthBarFloatingCanvas.gameObject.SetActive(true);
        bar[activeIdx].SetActive(false);
        interacting = false;
        inventoryMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        cameras[newActiveIdx].Priority = 10;
        players[newActiveIdx].SetActiveCharacter(true);
        players[newActiveIdx].healthBarFloatingCanvas.gameObject.SetActive(false);
        bar[newActiveIdx].SetActive(true);
        yield return new WaitForSeconds(1f);
        
        activeIdx = newActiveIdx;
        switching = false;

    }

    public void PlayerDie(Player playerObj) {
        int idxDel = -1;

        for (int i = 0; i < players.Count; i++) {
            if (playerObj == players[i]) {
                idxDel = i;
                Debug.Log(playerObj.gameObject);
                Debug.Log(idxDel);
                break;
            }
        }

        if (idxDel >= 0 && idxDel < players.Count) {
            Debug.Log("kedelete");

            if(players.Count == 1) {
                Debug.Log("dah kalah");
                GameManager.instance.EndGame(2);
            } else {
                int idxCurr = idxDel;
                bool isControlActive = false;

                if(idxCurr == newActiveIdx) {
                    isControlActive = true;
                } else {
                    for(int i = 0; i < players.Count; i++) {
                        if(i == newActiveIdx) {
                            idxCurr = i;
                        }
                    }  
                }

                players.RemoveAt(idxDel);
                cameras.RemoveAt(idxDel);
                bar[idxDel].SetActive(false);
                bar.RemoveAt(idxDel);
                Destroy(playerObj.gameObject);  
                GameObject explode = Instantiate(collides, playerObj.transform.position, playerObj.transform.rotation);
                Destroy(explode, 0.5f);

                if(isControlActive) {
                    newActiveIdx = 0;
                } else {
                    if(idxDel < idxCurr) {
                        newActiveIdx = idxCurr - 1;
                    } else {
                        newActiveIdx = idxCurr;
                    }
                }

                activeIdx = newActiveIdx;
                Debug.Log("new active = " + newActiveIdx);
                cameras[newActiveIdx].Priority = 10;
                players[newActiveIdx].SetActiveCharacter(true);
                players[newActiveIdx].healthBarFloatingCanvas.gameObject.SetActive(false);
                bar[newActiveIdx].SetActive(true);
            }
        }
    }
}
