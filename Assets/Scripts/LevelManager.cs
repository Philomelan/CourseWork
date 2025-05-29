using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    [SerializeField] private Transform[] path;

    [Header("Attributes")]
    [SerializeField] private int castleHealth;
    [SerializeField] private int levelNumber;
    [SerializeField] private float scaleCoef = 1f;

    private int enemiesKilled = 0;
    private int goldSpent = 0;
    private bool levelCompleted = false;

    private void Awake() {
        main = this;
        Time.timeScale = 1f;
    }

    private void Update() {
        if (castleHealth == 0 && !levelCompleted) {
            levelCompleted = true;
            OverlayManager.main.ShowDefeatScreen();
        }
    }

    public Transform[] GetPath() {
        return path;
    }

    public int GetCastleHealth() {
        return castleHealth;
    }

    public int GetEnemiesKilled() { 
        return enemiesKilled; 
    }

    public int GetGoldSpent() { 
        return goldSpent; 
    }

    public int GetLevelNumber() {
        return levelNumber;
    }

    public void DecreaseCastleHealth() {
        castleHealth--;
    }

    public void IncreaseEnemiesKilled() {
        enemiesKilled++;
    }

    public void IncreaseGoldSpent(int amount) {
        goldSpent += amount;
    }

    public float GetScaleCoef() {
        return scaleCoef;
    }
}
