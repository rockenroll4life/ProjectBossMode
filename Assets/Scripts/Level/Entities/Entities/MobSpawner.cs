using System;
using UnityEngine;

public class MobSpawner : Entity, Damageable {
    public GameObject MobPrefab;

    float health = 25;

    //  TEMP test variable
    bool hasSpawner = false;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override Type GetSystemType() => typeof(MobSpawner);

    public override bool IsDead() => health <= 0;

    protected override Color? GetHighlightColor() => Color.black;
    protected override Color? GetHighlightOutlineColor() => Color.black;

    private void Start() {
        Setup(FindObjectOfType<Level>());
    }

    public override void Setup(Level level) {
        base.Setup(level);

        Debug.Assert(MobPrefab, "NO MOB SET ON MOB SPAWNER!");
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        if (!hasSpawner) {
            hasSpawner = true;
            GetLevel().SpawnEntity(MobPrefab, transform.position, transform.rotation);
        }
    }

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Mob");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
