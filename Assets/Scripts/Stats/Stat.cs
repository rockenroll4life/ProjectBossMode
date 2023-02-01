using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;
public class Stat {
    public string name { get; private set; }
    private float _currentValue;
    public float currentValue {
        get => _currentValue;
        set {
            _currentValue = Mathf.Clamp(value, minValue, maxValue);

            //  Support for events when stats change (E.g. when your health changes so you can update the health bar)
            //  [Rock]: Not sure what we should be passing as a value, but we're going to pass the value as a percent scaled up so if can be an int (0.1526 => 1526)
            if (valueUpdatedEventID != -1) {
                EventManager.TriggerEvent(valueUpdatedEventID, (int) (GetPercent() * 10000));
            }
        }
    }

    readonly Dictionary<System.Guid, StatModifier> modifiers = new Dictionary<System.Guid, StatModifier>();
    int valueUpdatedEventID = -1;

    bool dirty = false;
    float baseValue;
    float cachedValue;

    readonly float minValue;
    readonly float maxValue;

    public Stat(string name, float baseValue, float minValue, float maxValue, int valueUpdatedEventID) {
        this.name = name;
        this.baseValue = baseValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.valueUpdatedEventID = valueUpdatedEventID;

        dirty = false;
        cachedValue = baseValue;
        _currentValue = baseValue;
    }

    public Stat(string name, float baseValue, float minValue, float maxValue)
        : this(name, baseValue, minValue, maxValue, -1) {
    }

    public void SetBaseValue(float value) {
        baseValue = value;
        dirty = true;
    }

    public void AddModifier(System.Guid id, StatModifier modifier) {
        dirty = true;
        modifiers[id] = modifier;
    }

    public void RemoveModifier(System.Guid id) {
        dirty = true;
        modifiers.Remove(id);
    }

    public float GetValue() {
        //  As an optimization we'll return the cached value for this
        if (!dirty) {
            return cachedValue;
        }

        //  If this stat is dirt then we'll want to fully recalculate the value
        cachedValue = baseValue;

        //  First loop through and add any additive modifiers
        foreach (StatModifier modifier in modifiers.Values) {
            if (modifier.modifierType == StatModifier.ModifierType.Additive) {
                cachedValue += modifier.value;
            }
        }

        //  Then loop through again and calculate the new multiplicative value
        float multiplier = 1;
        foreach (StatModifier modifier in modifiers.Values) {
            if (modifier.modifierType == StatModifier.ModifierType.Multiplicative) {
                multiplier += modifier.value;
            }
        }
        cachedValue *= multiplier;

        //  Clamp this within our Min/Max range
        cachedValue = Mathf.Clamp(cachedValue, minValue, maxValue);

        //  Now that we've recalculated this we're no longer dirty
        dirty = false;

        return cachedValue;
    }

    public void ResetCurrent() {
        currentValue = baseValue;
    }

    public void SetValueUpdatedEvent(int eventID) {
        this.valueUpdatedEventID = eventID;
    }

    public void RemoveEvent() {
        valueUpdatedEventID = -1;
    }

    public float GetPercent() {
        //  We're assuming base value isn't null...don't make us regret it!
        return Mathf.Clamp01(currentValue / baseValue);
    }

    public float GetInversePercent() {
        //  We're assuming base value isn't null...don't make us regret it!
        return 1.0f - GetPercent();
    }

    //  Operator overloads so we can treat this like a variable
    public static bool operator >(Stat a, float b) => a.currentValue > b;
    public static bool operator <(Stat a, float b) => a.currentValue < b;

    public static bool operator ==(Stat a, float b) => Mathf.Approximately(a.currentValue, b);
    public static bool operator !=(Stat a, float b) => !Mathf.Approximately(a.currentValue, b);

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return (obj as Stat) == this;
    }

    public override int GetHashCode() {
        unchecked {
            return currentValue.GetHashCode();
        }
    }
}
