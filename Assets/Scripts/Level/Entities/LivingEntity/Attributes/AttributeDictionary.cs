using System;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDictionary {
    readonly LivingEntity owner;
    readonly Dictionary<IAttribute, IAttributeInstance> attributeDictionary = new Dictionary<IAttribute, IAttributeInstance>();
    readonly Dictionary<IAttribute, Action<int>> attributeListeners = new Dictionary<IAttribute, Action<int>>();

    public AttributeDictionary(LivingEntity owner) {
        this.owner = owner;
    }

    public IAttributeInstance RegisterAttribute(IAttribute attribute) {
        if (attributeDictionary.ContainsKey(attribute)) {
            Debug.LogError(attribute.GetName() + " is already registered!");
        }
        
        IAttributeInstance instance = CreateAttributeInstance(attribute);
        attributeDictionary.Add(attribute, instance);

        return instance;
    }

    public void RegisterListener(IAttribute attribute, Action<int> listener) {
        if (attributeListeners.TryGetValue(attribute, out Action<int> listeners)) {
            listeners += listener;
            attributeListeners[attribute] = listeners;
        } else {
            listeners += listener;
            attributeListeners.Add(attribute, listeners);
        }
    }

    public IAttributeInstance CreateAttributeInstance(IAttribute attribute) {
        return new ModifiableAttributeInstance(this, attribute);
    }

    public IAttributeInstance GetInstance(IAttribute attribute) {
        return attributeDictionary.GetValueOrDefault(attribute);
    }

    public IEnumerable<IAttributeInstance> GetAttributes() {
        return attributeDictionary.Values;
    }

    public void OnAttributeModified(IAttributeInstance attributeInstance) {
        if (attributeListeners.TryGetValue(attributeInstance.GetAttribute(), out Action<int> listeners)) {
            int param = (int) (attributeInstance.GetValue() * 1000);

            listeners.Invoke(param);
        }
    }
}
