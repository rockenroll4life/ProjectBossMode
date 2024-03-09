using UnityEngine;
using System;
using RockUtils.GameEvents;

public class ResourceBars : MonoBehaviour {
    public ResourceBar[] bars = new ResourceBar[(int) EntityDataType._COUNT];

    public void Setup(LivingEntity owner, Action<EntityDataType, float> resourceValueChangedDelegate) {
        //  TODO: [Rock]: Update Setup to just take in the attribute enum instead of the attribute
        bars[(int) EntityDataType.Health].Setup(owner, AttributeTypes.HealthMax, EntityDataType.Health, resourceValueChangedDelegate);
        bars[(int) EntityDataType.Mana].Setup(owner, AttributeTypes.ManaMax, EntityDataType.Mana, resourceValueChangedDelegate);
    }
}
