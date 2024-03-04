using System;
using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

public enum ResourceType {
    Mana,
    Health,
    _COUNT
}

[Serializable]
public class ResourceBar {
    public Image barFill;
    public Text currentText;
    public Text maxText;

    //  TODO: [Rock]: Make the owner a LivingEntity instead of a player
    Player owner;
    ResourceType resourceType;
    IAttribute maxValueAttribute;
    GameEvents valueChangedGameEvent;
    Action<ResourceType, float> currentValueChangedDelegate;

    float currentValue;
    float maxValue;

    public float Current() => currentValue;
    public float Max() => maxValue;

    //  TODO: [Rock]: Owner should be able to be changed to LivingEntity, currently they don't have mana
    public void Setup(Player owner, ResourceType resourceType, IAttribute maxValueAttribute, GameEvents valueChangedGameEvent, Action<ResourceType, float> currentValueChangedDelegate) {
        this.owner = owner;
        this.resourceType = resourceType;
        this.maxValueAttribute = maxValueAttribute;
        this.valueChangedGameEvent = valueChangedGameEvent;
        this.currentValueChangedDelegate = currentValueChangedDelegate;

        float valueMax = owner.GetAttribute(maxValueAttribute).GetValue();
        UpdateMaxValue(valueMax);
        UpdateCurrentValue(valueMax);

        EventManager.StartListening(owner.GetEntityID(), (int) valueChangedGameEvent, CurrentValueChanged);
        owner.GetAttributes().RegisterListener(LivingEntitySharedAttributes.HEALTH_MAX, MaxValueChanged);
    }

    public void Breakdown() {
        EventManager.StopListening(owner.GetEntityID(), (int) valueChangedGameEvent, CurrentValueChanged);
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

    //  TODO: [Rock]: We shouldn't be asking the owner directly for it's health and mana, it should be changed via something and passed as
    //  the param to make it generic, specially if we add additional bars
    void CurrentValueChanged(int param) {
        if (valueChangedGameEvent == GameEvents.Health_Changed) {
            UpdateCurrentValue(owner.GetHealth());
        } else if (valueChangedGameEvent == GameEvents.Mana_Changed) {
            UpdateCurrentValue(owner.GetMana());
        }

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
