using UnityEngine;
using RockUtils.GameEvents;

public class UpdatingEntityData : IEntityData {
    LivingEntity owner;
    EntityDataType type;
    float value, maxValue;
    float updateValue = -1;

    public UpdatingEntityData(LivingEntity owner, EntityDataType type, float startingValue, AttributeTypes maxAttributeValue, AttributeTypes updatingAttribute = AttributeTypes.None) {
        this.owner = owner;
        this.type = type;

        value = startingValue;
        maxValue = owner.GetAttribute(maxAttributeValue).GetValue();

        if (updatingAttribute != AttributeTypes.None) {
            updateValue = owner.GetAttribute(updatingAttribute).GetValue();

            owner.GetAttributes().RegisterListener(maxAttributeValue, MaxAttributeChanged);
            owner.GetAttributes().RegisterListener(updatingAttribute, UpdatingAttributeChanged);
        }
    }

    public float Get() => value;
    public void Set(float value) {
        this.value = value;
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Entity_Data_Changed + (int) type, (int) (value * 1000));
    }

    public void Update() {
        value += (updateValue * Time.deltaTime);
        value = Mathf.Clamp(value, 0, maxValue);
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Entity_Data_Changed + (int) type, (int) (value * 1000));
    }

    void MaxAttributeChanged(int param) {
        maxValue = param / 1000f;
    }

    void UpdatingAttributeChanged(int param) {
        updateValue = param / 1000f;
    }
}
