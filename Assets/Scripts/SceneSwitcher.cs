using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher main;

    private void Awake() {
        main = this;
    }

    public void OpenMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenSettings() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SettingsScene");
    }

    public void OpenTutorial() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TutorialScene");
    }

    public void OpenLevelSelect() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelectScene");
    }
    
    public void OpenLevel1() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1Scene");
    }
    
    public void OpenLevel2() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2Scene");
    }

    public void OpenLevel3() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level3Scene");
    }

    public void ReloadScene() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
