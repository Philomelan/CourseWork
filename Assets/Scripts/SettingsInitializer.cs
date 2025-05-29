using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsInitializer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;
    private float originalRatio = 1.6f;

    private void Start() {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        SetResolution(savedResolutionIndex);
        Screen.fullScreen = savedFullscreen;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(savedMusicVolume, 0.0001f, 1f)) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(savedSFXVolume, 0.0001f, 1f)) * 20);      
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];

        int width = resolution.width;
        int height = resolution.height;
        float ratio = (float)width / (float)height;
        if (ratio > originalRatio) {
            width = (int)(height * originalRatio);
        } else if (ratio < originalRatio) {
            height = (int)(width / originalRatio);
        }
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
