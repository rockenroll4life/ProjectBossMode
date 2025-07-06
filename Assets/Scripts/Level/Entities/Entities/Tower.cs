using System;
using UnityEngine;

public class Tower : Entity, IDamageable {
    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override Type GetSystemType() => typeof(Tower);

    protected override Color? GetHighlightColor() => Color.yellow;
    protected override Color? GetHighlightOutlineColor() => Color.yellow;

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(AttributeTypes.HealthMax).SetBaseValue(1000);
        SetEntityData(EntityDataType.Health, GetAttribute(AttributeTypes.HealthMax).GetValue());
    }

    public void Hurt(Entity damager, float damage) {
        SetEntityData(EntityDataType.Health, GetEntityData(EntityDataType.Health) - damage);

        Debug.Log($"{name} Health: {GetEntityData(EntityDataType.Health)}");
    }
}
