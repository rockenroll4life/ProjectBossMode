using System;
using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

public enum ResourceType {
    Health,
    Mana,
    _COUNT
}

public class ResourceBar : MonoBehaviour {
    public Image barFill;
    public Text currentText;
    public Text maxText;

    //  TODO: [Rock]: Make the owner a LivingEntity instead of a player
    LivingEntity owner;
    ResourceType resourceType;
    GameEvents valueChangedGameEvent;
    Action<ResourceType, float> currentValueChangedDelegate;

    float currentValue;
    float maxValue;

    public float Current() => currentValue;
    public float Max() => maxValue;

    public void Setup(LivingEntity owner, ResourceType resourceType, AttributeTypes maxValueAttribute, GameEvents valueChangedGameEvent, Action<ResourceType, float> currentValueChangedDelegate) {
        this.owner = owner;
        this.resourceType = resourceType;
        this.valueChangedGameEvent = valueChangedGameEvent;
        this.currentValueChangedDelegate = currentValueChangedDelegate;

        float valueMax = owner.GetAttribute(maxValueAttribute).GetValue();
        UpdateMaxValue(valueMax);
        UpdateCurrentValue(valueMax);

        EventManager.StartListening(owner.GetEntityID(), valueChangedGameEvent, CurrentValueChanged);
        owner.GetAttributes().RegisterListener(AttributeTypes.HealthMax, MaxValueChanged);
    }

    public void Breakdown() {
        EventManager.StopListening(owner.GetEntityID(), valueChangedGameEvent, CurrentValueChanged);
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

    void CurrentValueChanged(int param) {
        UpdateCurrentValue(owner.GetResource(resourceType));

        if (currentValueChangedDelegate != null) {
            currentValueChangedDelegate(resourceType, currentValue);
        }
    }

    void MaxValueChanged(int param) {
        float maxValue = (int) (param / 1000f);
        UpdateMaxValue(maxValue);

        if (currentValueChangedDelegate != null) {
            currentValueChangedDelegate(resourceType, currentValue);
        }
    }
}
