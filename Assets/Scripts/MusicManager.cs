using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager main;
    private AudioSource audioSource;

    private void Start() {
        if (main != null && main != this) {
            Destroy(gameObject);
            return;
        }
        main = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();        
    }

    void Update() {
        if (!(SceneManager.GetActiveScene().name == "MainMenuScene" || SceneManager.GetActiveScene().name == "SettingsScene" 
            || SceneManager.GetActiveScene().name == "LevelSelectScene" || SceneManager.GetActiveScene().name == "TutorialScene")) {
            Destroy(gameObject);
        }

        if (!audioSource.isPlaying) {
            audioSource.Play();
        }        
    }
}
