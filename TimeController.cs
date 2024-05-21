using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    public TextMeshProUGUI timeText;
    private TimeSpan play;
    private bool timeOn;
    private float elapsedTime;

    public float ElapsedTime {
        get {
           return  elapsedTime;
        }
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        timeText.SetText("00:00");
        timeOn = false;
    }

    public void startTime() {
        timeOn = true;
        elapsedTime = 0f;
        StartCoroutine(updateTime());
    }

    public void endTime() {
        timeOn = false;
    }

    private IEnumerator updateTime() {
        while(timeOn) {
            elapsedTime += Time.deltaTime;
            play = TimeSpan.FromSeconds(elapsedTime);
            string playing = play.ToString("mm':'ss");
            timeText.SetText(playing);
            
            yield return null;
        }
    }
}
