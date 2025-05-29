using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<TutorialStep> steps;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TMP_Text tutorialText;

    private int currentIndex = 0;

    void Start() {
        ShowStep(currentIndex);

        nextButton.onClick.AddListener(NextStep);
        prevButton.onClick.AddListener(PrevStep);
    }

    void ShowStep(int index) {
        if (index >= 0 && index < steps.Count) {
            tutorialImage.sprite = steps[index].image;
            tutorialText.text = steps[index].description;

            AspectRatioFitter fitter = tutorialImage.GetComponent<AspectRatioFitter>();
            fitter.aspectRatio = (float)tutorialImage.sprite.texture.width / tutorialImage.sprite.texture.height;

                prevButton.interactable = index > 0;
            nextButton.interactable = index < steps.Count - 1;
        }
    }

    private void NextStep() {
        if (currentIndex < steps.Count - 1) {
            currentIndex++;
            ShowStep(currentIndex);
        }
    }

    private void PrevStep() {
        if (currentIndex > 0) {
            currentIndex--;
            ShowStep(currentIndex);
        }
    }
}
