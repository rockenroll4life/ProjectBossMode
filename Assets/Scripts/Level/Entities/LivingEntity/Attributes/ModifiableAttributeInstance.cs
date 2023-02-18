using System;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableAttributeInstance : IAttributeInstance {
    readonly AttributeDictionary attributeDictionary;
    readonly Dictionary<AttributeModifier.Operation, Dictionary<Guid, AttributeModifier>> modifiers = new Dictionary<AttributeModifier.Operation, Dictionary<Guid, AttributeModifier>>();
    readonly IAttribute attribute;
    float baseValue;
    float cachedValue;
    bool dirty = true;

    public ModifiableAttributeInstance(AttributeDictionary attributeDictionary, IAttribute attribute) {
        this.attributeDictionary = attributeDictionary;
        this.attribute = attribute;

        baseValue = attribute.GetDefaultValue();

        for (int i = 0; i < (int) AttributeModifier.Operation._TOTAL; i++) {
            modifiers.Add((AttributeModifier.Operation) i, new Dictionary<Guid, AttributeModifier>());
        }
    }

    public IAttribute GetAttribute() {
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

        foreach (AttributeModifier modifier in GetModifiers(AttributeModifier.Operation.Addition)) {
            value += modifier.GetAmount();
        }

        float result = value;

        foreach (AttributeModifier modifier in GetModifiers(AttributeModifier.Operation.Multiply_Base)) {
            result += value * modifier.GetAmount();
        }

        foreach (AttributeModifier modifier in GetModifiers(AttributeModifier.Operation.Multiply_Total)) {
            result *= 1 + modifier.GetAmount();
        }

        //  Lastly, make sure the value is cleaned up and within any bounds that it needs to be within
        return attribute.CleanupValue(result);
    }

    public void AddModifier(AttributeModifier modifier) {
        if (modifiers[modifier.GetOperation()].ContainsKey(modifier.GetID())) {
            Debug.LogException(new Exception("This modifier has already been added!"));
        }

        modifiers[modifier.GetOperation()].Add(modifier.GetID(), modifier);

        SetDirty();
    }

    public void RemoveModifier(AttributeModifier modifier) {
        if (!modifiers[modifier.GetOperation()].ContainsKey(modifier.GetID())) {
            Debug.LogException(new Exception("This modifier does not exist!"));
        }

        modifiers[modifier.GetOperation()].Remove(modifier.GetID());

        SetDirty();
    }

    IEnumerable<AttributeModifier> GetModifiers(AttributeModifier.Operation operation) {
        return modifiers[operation].Values;
    }
}
