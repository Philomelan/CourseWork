using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    private void Start()
    {
        bool level1Passed = PlayerPrefs.GetInt("Level1Completed", 0) == 1;
        bool level2Passed = PlayerPrefs.GetInt("Level2Completed", 0) == 1;
        bool level3Passed = PlayerPrefs.GetInt("Level3Completed", 0) == 1;
                
        level2Button.interactable = level1Passed;
        level3Button.interactable = level2Passed;
    }
}
