using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool pause = false;
    public bool isNotTown;
    public GameObject pauseMenu;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(pause) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Menu() {
        Time.timeScale = 1f;
        if(isNotTown) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        AudioListener.pause = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
