using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager main;

    [Header("References")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject confirmUI;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject defeatUI;
    [SerializeField] private GameObject darkOverlay;

    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip defeatClip;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;

    private bool isPaused = false;
    private bool LevelCompleted = false;
    private TextMeshProUGUI goldSpentText;
    private TextMeshProUGUI enemiesKilledText;
    public bool overlayOpened = false;

    private void Awake() {
        main = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !LevelCompleted) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        pauseUI.SetActive(false);
        darkOverlay.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        overlayOpened = false;
    }

    public void PauseGame() {
        pauseUI.SetActive(true);
        darkOverlay.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        overlayOpened = true;

        SyncSliders(musicVolumeSlider, SFXVolumeSlider);
    }

    public void ConfirmQuit() {
        pauseUI.SetActive(false);
        confirmUI.SetActive(true);
    }

    public void CancelConfirmation() {
        pauseUI.SetActive(true);
        confirmUI.SetActive(false);
    }

    public void ShowVictoryScreen(int levelNumber) {
        victoryUI.SetActive(true);
        darkOverlay.SetActive(true);
        musicSource.GetComponent<AudioSource>().Pause();
        SFXManager.main.PlaySFX(victoryClip);
        Time.timeScale = 0f;
        LevelCompleted = true;
        overlayOpened = true;

        goldSpentText = victoryUI.transform.Find("Panel/GoldSpentValue").GetComponent<TextMeshProUGUI>();
        enemiesKilledText = victoryUI.transform.Find("Panel/EnemiesKilledValue").GetComponent<TextMeshProUGUI>();
        goldSpentText.text = LevelManager.main.GetGoldSpent().ToString();
        enemiesKilledText.text = LevelManager.main.GetEnemiesKilled().ToString();

        PlayerPrefs.SetInt($"Level{levelNumber}Completed", 1);
        PlayerPrefs.Save();
    }

    public void ShowDefeatScreen() {
        defeatUI.SetActive(true);
        darkOverlay.SetActive(true);
        musicSource.GetComponent<AudioSource>().Pause();
        SFXManager.main.PlaySFX(defeatClip);
        Time.timeScale = 0f;
        overlayOpened = true;

        goldSpentText = defeatUI.transform.Find("Panel/GoldSpentValue").GetComponent<TextMeshProUGUI>();
        enemiesKilledText = defeatUI.transform.Find("Panel/EnemiesKilledValue").GetComponent<TextMeshProUGUI>();
        goldSpentText.text = LevelManager.main.GetGoldSpent().ToString();
        enemiesKilledText.text = LevelManager.main.GetEnemiesKilled().ToString();
    }

    public void SyncSliders(Slider musicSlider, Slider sfxSlider) {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
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

    public bool GetOverlayOpened() {
        return overlayOpened;
    }
}
