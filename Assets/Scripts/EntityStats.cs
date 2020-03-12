using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour {
    public readonly Stat HEALTH = new Stat("stat.generic.health", 100, 0, float.MaxValue);
    public readonly Stat MOVEMENT_SPEED = new Stat("stat.generic.movementSpeed", 13.2f, 0, float.MaxValue);
}

public class Stat {
    public string name { get; private set; }
    public float baseValue { get; private set; }

    readonly Dictionary<UnityEditor.GUID, StatModifier> modifiers = new Dictionary<UnityEditor.GUID, StatModifier>();
    bool dirty = false;
    float cachedValue;

    readonly float minValue;
    readonly float maxValue;

    public Stat(string name, float baseValue, float minValue, float maxValue) {
        this.name = name;
        this.baseValue = baseValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        
        dirty = false;
        cachedValue = baseValue;
    }

    public void SetBaseValue(float value) {
        baseValue = value;
        dirty = true;
    }

    public void AddModifier(UnityEditor.GUID id, StatModifier modifier) {
        dirty = true;
        modifiers[id] = modifier;
    }

    public void RemoveModifier(UnityEditor.GUID id) {
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
}

public class StatModifier {
    public enum ModifierType {
        Additive,
        Multiplicative
    }

    public ModifierType modifierType { get; private set; }
    public float value { get; private set; }

    public StatModifier(ModifierType modifierType, float value) {
        this.modifierType = modifierType;
        this.value = value;
    }
}