using RockUtils.GameEvents;
using System;
using UnityEngine;

public class MobSpawner : Entity, IDamageable {
    public GameObject MobPrefab;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override Type GetSystemType() => typeof(MobSpawner);

    protected override Color? GetHighlightColor() => Color.black;
    protected override Color? GetHighlightOutlineColor() => Color.black;

    public override void Setup(Level level) {
        base.Setup(level);

        Debug.Assert(MobPrefab, "NO MOB SET ON MOB SPAWNER!");
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(GameEvents.MobSpawner_SpawnEntities, SpawnEntity);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(GameEvents.MobSpawner_SpawnEntities, SpawnEntity);
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(AttributeTypes.HealthMax).SetBaseValue(1000);
        SetEntityData(EntityDataType.Health, GetAttribute(AttributeTypes.HealthMax).GetValue());
    }

    public void Hurt(Entity damager, float damage) {
        SetEntityData(EntityDataType.Health, GetEntityData(EntityDataType.Health) - damage);

        Debug.Log($"{name} Health: {GetEntityData(EntityDataType.Health)}");
    }

    private void SpawnEntity(int param) {
        GetLevel().SpawnEntity(MobPrefab, transform.position, transform.rotation);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Mob");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
