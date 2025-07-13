using System;
using UnityEngine;

public class MobSpawner : Entity, IDamageable {
    public GameObject MobPrefab;

    //  TEMP test variable
    bool hasSpawned = false;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override Type GetSystemType() => typeof(MobSpawner);

    protected override Color? GetHighlightColor() => Color.black;
    protected override Color? GetHighlightOutlineColor() => Color.black;

    public override void Setup(Level level) {
        base.Setup(level);

        Debug.Assert(MobPrefab, "NO MOB SET ON MOB SPAWNER!");
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(AttributeTypes.HealthMax).SetBaseValue(1000);
        SetEntityData(EntityDataType.Health, GetAttribute(AttributeTypes.HealthMax).GetValue());
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        if (!hasSpawned) {
            hasSpawned = true;
            GetLevel().SpawnEntity(MobPrefab, transform.position, transform.rotation);
        }
    }

    public void Hurt(Entity damager, float damage) {
        SetEntityData(EntityDataType.Health, GetEntityData(EntityDataType.Health) - damage);

        Debug.Log($"{name} Health: {GetEntityData(EntityDataType.Health)}");
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Mob");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
