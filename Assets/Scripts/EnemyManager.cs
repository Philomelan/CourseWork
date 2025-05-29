using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework;
using static UnityEngine.InputSystem.InputControlExtensions;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager main;

    [Header("References")]
    [SerializeField] private Enemy[] enemies;

    [Header("Attributes")]
    [SerializeField] private int timeBetweenWaves = 15;
    [SerializeField] private float baseSpawnSpeed = 0.9f;
    
    private bool isWave = false;
    private int waveNumber = 0;
    private float spawnDelay;
    private int enemiesAlive;
    private bool spawnFinished = false;
    private float countdownTimer;
    private int[,] enemyDistribution;
    private int flipX = 1;

    private void Awake() {
        main = this;
        countdownTimer = timeBetweenWaves;   
    }

    private void Start() {
        CreateEnemyDistribution();
    }

    private void Update() {
        if (!isWave) {
            if (countdownTimer > 0) {
                countdownTimer -= Time.deltaTime;
                return;
            } else {
                countdownTimer = 0;
                StartCoroutine(StartWave());
            }
        }

        if (enemiesAlive == 0 && spawnFinished) {
            EndWave();
        }
    }

    private IEnumerator StartWave() {
        isWave = true;
        spawnFinished = false;
        waveNumber++;
        UIManager.main.SwitchEnemyDirectionSign();

        baseSpawnSpeed *= Mathf.Pow(waveNumber, 0.5f);
        baseSpawnSpeed = Mathf.Min(baseSpawnSpeed, 1.75f);
        spawnDelay = (1f / baseSpawnSpeed);

        List<int> spawnOrder = new List<int>();
        for (int i = 0; i < enemyDistribution.GetLength(1); i++)
            for (int j = 0; j < enemyDistribution[waveNumber - 1, i]; j++)
                spawnOrder.Add(i);

        for (int i = spawnOrder.Count - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1);
            (spawnOrder[i], spawnOrder[j]) = (spawnOrder[j], spawnOrder[i]);
        }

        foreach (int index in spawnOrder) {
            if (LevelManager.main.GetLevelNumber() == 3) {
                flipX = Random.value < 0.5f ? 1 : -1;
            }
            Vector3 spawnPosition = LevelManager.main.GetPath()[0].position;
            spawnPosition.x *= flipX;

            GameObject spawnedEnemy = Instantiate(enemies[index].prefab, spawnPosition, Quaternion.identity);
            spawnedEnemy.transform.localScale *= LevelManager.main.GetScaleCoef();
            spawnedEnemy.GetComponent<EnemyBehaviour>().SetFlipX(flipX);
            enemiesAlive++;
            yield return new WaitForSeconds(spawnDelay);
        }

        spawnFinished = true;
    }

    private void EndWave() {
        isWave = false;

        if (waveNumber == enemyDistribution.GetLength(0)) {
            OverlayManager.main.ShowVictoryScreen(LevelManager.main.GetLevelNumber());
        } else {
            countdownTimer = timeBetweenWaves;
        }

        UIManager.main.SwitchEnemyDirectionSign();
    }

    private void CreateEnemyDistribution() {
        if (LevelManager.main.GetLevelNumber() == 1) {
            enemyDistribution = new int[8, 6] 
                    {{5, 0, 0, 0, 0, 0},
                    {5, 2, 0, 0, 0, 0},
                    {5, 5, 2, 0, 0, 0},
                    {8, 5, 5, 0, 0, 0},
                    {8, 3, 8, 0, 0, 0},
                    {8, 3, 5, 2, 0, 0},
                    {8, 3, 8, 3, 0, 0},
                    {10, 8, 8, 4, 0, 0}};
        } else if (LevelManager.main.GetLevelNumber() == 2) {
            enemyDistribution = new int[10, 6]
                    {{5, 0, 0, 0, 0, 0},
                    {8, 0, 0, 0, 0, 0},
                    {8, 2, 0, 0, 0, 0},
                    {5, 5, 3, 0, 0, 0},
                    {5, 3, 5, 2, 0, 0},
                    {5, 8, 5, 5, 0, 0},
                    {0, 3, 8, 5, 1, 0},
                    {0, 5, 8, 5, 1, 1},
                    {0, 3, 8, 10, 2, 0},
                    {0, 5, 8, 10, 2, 1}};
        } else {
            enemyDistribution = new int[10, 6]
                    {{5, 0, 0, 0, 0, 0},
                    {5, 3, 2, 0, 0, 0},
                    {8, 3, 5, 0, 0, 0},
                    {8, 5, 5, 2, 0, 0},
                    {0, 3, 5, 3, 0, 0},
                    {0, 8, 5, 3, 1, 0},
                    {3, 3, 5, 5, 1, 1},
                    {3, 5, 8, 5, 2, 1},
                    {5, 3, 10, 10, 3, 1},
                    {5, 8, 10, 10, 3, 3}};
        }
        return;
    }

    public void DestroyEnemy() {
        enemiesAlive--;
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetCountdownTimer() {
        return countdownTimer;
    }

    public void ResetCountdownTimer() {
        countdownTimer = 0;
    }

    public float GetNumberOfWaves() {
        return enemyDistribution.GetLength(0);
    }
}
