using UnityEngine;
using System;
using RockUtils.GameEvents;

public class ResourceBars : MonoBehaviour {
    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];

    public void Setup(LivingEntity owner, Action<ResourceType, float> resourceValueChangedDelegate) {
        //  TODO: [Rock]: Update Setup to just take in the attribute enum instead of the attribute
        bars[(int) ResourceType.Health].Setup(owner, ResourceType.Health, Attributes.Get(AttributeTypes.HealthMax), GameEvents.Health_Changed, resourceValueChangedDelegate);
        bars[(int) ResourceType.Mana].Setup(owner, ResourceType.Mana, Attributes.Get(AttributeTypes.ManaMax), GameEvents.Mana_Changed, resourceValueChangedDelegate);
    }

    public void Breakdown() {
        foreach (ResourceBar bar in bars) {
            bar.Breakdown();
        }
    }
}
