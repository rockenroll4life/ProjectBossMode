using System;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType {
    Health = 0,
    Mana = 1,
    _COUNT = 2
}

[Serializable]
public class ResourceBar {
    public Image barFill;
    public Text currentText;
    public Text maxText;

    float currentValue;
    float maxValue;

    public float Current() => currentValue;
    public float Max() => maxValue;

    public void Setup(float valueMax) {
        UpdateMaxValue(valueMax);
        UpdateCurrentValue(valueMax);
    }

    public void UpdateCurrentValue(float value) {
        currentValue = value;
        UpdateText();
    }

    public void UpdateMaxValue(float value) {
        maxValue = value;
        currentValue = Mathf.Min(currentValue, maxValue);
        UpdateText();
    }

    public void UpdateText() {
        currentText.text = ((int) currentValue).ToString();
        maxText.text = ((int) maxValue).ToString();
        barFill.fillAmount = currentValue / maxValue;
    }
}
