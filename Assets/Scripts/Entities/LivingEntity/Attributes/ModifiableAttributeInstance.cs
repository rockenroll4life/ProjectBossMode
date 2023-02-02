using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableAttributeInstance : AttributeInstance {
    readonly AttributeDictionary attributeDictionary;
    readonly Attribute attribute;
    float baseValue;
    float cachedValue;
    bool dirty = true;

    public ModifiableAttributeInstance(AttributeDictionary attributeDictionary, Attribute attribute) {
        this.attributeDictionary = attributeDictionary;
        this.attribute = attribute;

        baseValue = attribute.GetDefaultValue();
    }

    public Attribute GetAttribute() {
        return attribute;
    }

    void SetDirty() {
        dirty = true;
        attributeDictionary.OnAttributeModified(this);
    }

    public float GetBaseValue() {
        return baseValue;
    }

    public float GetValue() {
        if (dirty) {
            cachedValue = CalculateValue();
            dirty = false;
        }

        return cachedValue;
    }

    public void SetBaseValue(float baseValue) {
        if (baseValue == GetBaseValue()) {
            return;
        }

        this.baseValue = baseValue;
        SetDirty();
    }

    float CalculateValue() {
        float value = GetBaseValue();

        //  TODO: [Rock]: Implement Attribute Modifiers - Addition, Multiply Base, Multiply Total

        //  Loop through all the Addition modifiers and add them to value
        //  value += modifier.Amount();

        float result = value;

        //  Loops through all the Multiply Base modifiers
        //  result += value * modifier.Amount();

        //  Loop through all the Multiply Total modifiers
        // result *= 1 + modifier.Amount();

        //  Lastly, make sure the value is cleaned up and within any bounds that it needs to be within
        return attribute.CleanupValue(result);
    }
}
