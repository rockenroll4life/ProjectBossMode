using UnityEngine;

public class Tower : Entity, Damageable {
    float health = 25;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;
    public override bool IsDead() => health <= 0;

    protected override Color? GetHighlightColor() => Color.yellow;
    protected override Color? GetHighlightOutlineColor() => Color.yellow;

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);
    }
}
