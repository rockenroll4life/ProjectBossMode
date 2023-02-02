using System.Collections.Generic;
using UnityEngine;

public class AttributeDictionary {
    readonly Dictionary<Attribute, AttributeInstance> attributeDictionary = new Dictionary<Attribute, AttributeInstance>();

    public AttributeInstance RegisterAttribute(Attribute attribute) {
        if (attributeDictionary.ContainsKey(attribute)) {
            Debug.LogError(attribute.GetName() + " is already registered!");
        }
        
        AttributeInstance instance = CreateAttributeInstance(attribute);
        attributeDictionary.Add(attribute, instance);

        return instance;
    }

    public AttributeInstance CreateAttributeInstance(Attribute attribute) {
        return new ModifiableAttributeInstance(this, attribute);
    }

    public AttributeInstance GetInstance(Attribute attribute) {
        return attributeDictionary.GetValueOrDefault(attribute);
    }

    public ICollection<AttributeInstance> GetAttributes() {
        return attributeDictionary.Values;
    }
}
