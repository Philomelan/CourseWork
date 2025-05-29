using UnityEngine;
using System;

[Serializable]
public class TutorialStep
{
    public Sprite image;
    [TextArea] public string description;

    public TutorialStep(Sprite image, string description) {
        this.image = image;
        this.description = description;
    }
}
