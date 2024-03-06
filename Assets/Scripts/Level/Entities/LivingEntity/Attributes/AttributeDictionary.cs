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

    public IAttributeInstance RegisterAttribute(AttributeTypes attribute) {
        IAttribute iAttribute = Attributes.Get(attribute);
        if (attributeDictionary.ContainsKey(iAttribute)) {
            Debug.LogError(iAttribute.GetName() + " is already registered!");
        }
        
        IAttributeInstance instance = CreateAttributeInstance(iAttribute);
        attributeDictionary.Add(iAttribute, instance);

        return instance;
    }

    public void RegisterListener(AttributeTypes attribute, Action<int> listener) {
        IAttribute iattribute = Attributes.Get(attribute);

        if (attributeListeners.TryGetValue(iattribute, out Action<int> listeners)) {
            listeners += listener;
            attributeListeners[iattribute] = listeners;
        } else {
            listeners += listener;
            attributeListeners.Add(iattribute, listeners);
        }
    }

    public void UnregisterListener(AttributeTypes attribute, Action<int> listener) {
        IAttribute iattribute = Attributes.Get(attribute);

        if (attributeListeners.TryGetValue(iattribute, out Action<int> listeners)) {
            listeners -= listener;
            attributeListeners[iattribute] = listeners;
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
