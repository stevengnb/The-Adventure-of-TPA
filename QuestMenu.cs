using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class QuestMenu : MonoBehaviour
{
    public float durationTransition = 13f;
    public GameObject ShowInteract;
    public GameObject Dark;
    public PostProcessVolume postProcessVolume;
    public AudioSource BackgroundMusic;
    public AudioSource DarkBackgroundMusic;
    private TimAnimationController timAnimation;
    
    public void Start() {
        GameObject knight = GameObject.Find("Knight");
        timAnimation = knight.GetComponent<TimAnimationController>();
    }

    public void StartQuests() {
        ShowInteract.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        postProcessVolume.weight = 0f;
        Dark.SetActive(true);
        DarkBackgroundMusic.Play();
        BackgroundMusic.Pause();
        timAnimation.ChangeScene();
        StartCoroutine(DarkTransition(postProcessVolume.weight, 1f, durationTransition));
    }

    private IEnumerator DarkTransition(float initial, float target, float durationTransition) {
        float initialWeight = initial;
        float targetWeight = target;
        float elapsedTime = 0f;
        float initialMusicVolume = DarkBackgroundMusic.volume;

        while (elapsedTime < durationTransition) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / durationTransition);

            postProcessVolume.weight = Mathf.Lerp(initialWeight, targetWeight, t);
            DarkBackgroundMusic.volume = Mathf.Lerp(initialMusicVolume, targetWeight, t);

            yield return null;
        }

        postProcessVolume.weight = targetWeight;
        DarkBackgroundMusic.volume = targetWeight;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
