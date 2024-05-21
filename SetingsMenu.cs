using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIdx) {
        QualitySettings.SetQualityLevel(qualityIdx);
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }
} 
