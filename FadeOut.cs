using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float durationTransition = 10f;
    public GameObject panel;
    public GameObject crystal;
    public GameObject player;
    public Image image;
    public AudioSource rainSound;
    public AudioSource backgroundSound;
    public AudioSource gameOver;

    public void Fade() {
        image = panel.GetComponent<Image>();
        Time.timeScale = 0.5f;
        rainSound.volume = 0;
        backgroundSound.volume = 0;
        gameOver.Play();
        StartCoroutine(DarkTransition(0, 1, durationTransition));
    }

    public void GameOverType(int type){
        if(type == 1) {
            crystal.SetActive(true);
        } else if(type == 2) {
            player.SetActive(true);
        }
    }

    private IEnumerator DarkTransition(float initial, float target, float duration) {
        Color initialColor = image.color;
        Color targetColor = image.color;
        initialColor.a = initial;
        targetColor.a = target;

        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            image.color = Color.Lerp(initialColor, targetColor, t);

            yield return null;
        }

        image.color = targetColor;
        yield return StartCoroutine(ChangeScene(1f));        
    }

    private IEnumerator ChangeScene(float duration) {
        yield return new WaitForSeconds(duration);
        GameManager.instance.isEndGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
