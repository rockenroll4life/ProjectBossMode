using UnityEngine;
using System;
using RockUtils.GameEvents;

public class ResourceBars : MonoBehaviour {
    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];

    public void Setup(LivingEntity owner, Action<ResourceType, float> resourceValueChangedDelegate) {
        bars[(int) ResourceType.Health].Setup(owner, ResourceType.Health, LivingEntitySharedAttributes.HEALTH_MAX, GameEvents.Health_Changed, resourceValueChangedDelegate);
        bars[(int) ResourceType.Mana].Setup(owner, ResourceType.Mana, LivingEntitySharedAttributes.MANA_MAX, GameEvents.Mana_Changed, resourceValueChangedDelegate);
    }

    public void Breakdown() {
        foreach (ResourceBar bar in bars) {
            bar.Breakdown();
        }
    }
}
