using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RetryGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void BackGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

}