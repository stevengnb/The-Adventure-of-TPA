using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwordInteraction : MonoBehaviour
{

    private bool interacting = false;
    private bool inRange = false;
    public float durationShow = 10f;
    public float durationTransition = 0.001f;
    public GameObject ShowInteract;
    public GameObject Dark;
    public PostProcessVolume postProcessVolume;
    public AudioSource BackgroundMusic;
    public AudioSource DarkBackgroundMusic;

    public bool Interacting {
        get { return interacting; }
        set { interacting = value; }
    }
    private void Update() {
        if(inRange && Input.GetKeyUp(KeyCode.F)) {
            if (!interacting) {
                interacting = true;
                postProcessVolume.weight = 0f;
                Dark.SetActive(true);
                DarkBackgroundMusic.Play();
                BackgroundMusic.Pause();
                StartCoroutine(DarkTransition(postProcessVolume.weight, 0.9f, durationTransition));
                StartCoroutine(DeactivateScene(durationShow, durationTransition));
            }
        }   
    }

    private IEnumerator DeactivateScene(float duration, float durationTransition) {
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(DarkTransition(postProcessVolume.weight, 0f, durationTransition));
    
        interacting = false;
        BackgroundMusic.Play();
        DarkBackgroundMusic.Stop();
        Dark.SetActive(false);
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
    }

    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            ShowInteract.SetActive(false);
            if(!interacting) {
                ShowInteract.SetActive(true);
            }
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        ShowInteract.SetActive(false);
        inRange = false;
    }
}
