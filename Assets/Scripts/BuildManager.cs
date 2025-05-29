using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private Button[] buttons;

    [Header("Attributes")]
    [SerializeField] private int currency = 100;

    private int selectedTower = 0;


    private void Awake() {
        main = this;
    }

    private void Update() {
        if (OverlayManager.main.GetOverlayOpened()) {
            foreach (Button button in buttons) {
                button.enabled = false;
            }
        } else {
            foreach (Button button in buttons) {
                button.enabled = true;
            }
        }
    }

    public void IncreaseCurrency(int amount) {
        currency += amount;
        if (currency > 9999) {
            currency = 9999;
        }
    }

    public bool DecreaseCurrency(int amount) {
        if (amount <= currency) {
            currency -= amount;
            return true;
        } else {
            return false;
        }
    }

    public Tower GetSelectedTower() {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int selectedTower) {
        this.selectedTower = selectedTower;
    } 

    public int GetCurrency() {
        return currency;
    }

    public Tower GetTower(int index) {
        return towers[index];
    }
}
