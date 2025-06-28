using System;
using UnityEngine;

public class Tower : Entity, IDamageable {
    float health = 25;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override Type GetSystemType() => typeof(Tower);
    public override bool IsDead() => health <= 0;

    protected override Color? GetHighlightColor() => Color.yellow;
    protected override Color? GetHighlightOutlineColor() => Color.yellow;

    public void Hurt(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);
    }
}
