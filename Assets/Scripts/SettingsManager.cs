using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;
    private float originalRatio = 1.6f;

    private void Start() {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;        

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {       
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = savedFullscreen;
        musicVolumeSlider.value = savedMusicVolume;
        SFXVolumeSlider.value = savedSFXVolume;
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

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void ResetProgress() {
        PlayerPrefs.SetInt($"Level1Completed", 0);
        PlayerPrefs.SetInt($"Level2Completed", 0);
        PlayerPrefs.SetInt($"Level3Completed", 0);
        PlayerPrefs.Save();

        SceneSwitcher.main.ReloadScene();
    }

    public void ResetSettings() {
        resolutionDropdown.value = Screen.resolutions.Length - 1;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = true;
        musicVolumeSlider.value = 1;
        SFXVolumeSlider.value = 1;

        SceneSwitcher.main.ReloadScene();
    }
}
