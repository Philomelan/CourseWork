using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{
    public static UIManager main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI castleHealthText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI timeBeforeWaveText;

    [SerializeField] private List<Image> enemyDirectionSigns;

    private void Awake() {
        main = this;
    }

    private void OnGUI() {
        castleHealthText.text = LevelManager.main.GetCastleHealth().ToString();
        currencyText.text = BuildManager.main.GetCurrency().ToString();
        waveNumberText.text = EnemyManager.main.GetWaveNumber().ToString() + "/" + EnemyManager.main.GetNumberOfWaves().ToString();
        timeBeforeWaveText.text = Mathf.RoundToInt(EnemyManager.main.GetCountdownTimer()).ToString();
    }

    public void SwitchEnemyDirectionSign() {
        foreach (Image sign in enemyDirectionSigns) {
            sign.enabled = !sign.enabled;
        }
    }

    public void StartWaveEarlier() {
        EnemyManager.main.ResetCountdownTimer();
    }
}
