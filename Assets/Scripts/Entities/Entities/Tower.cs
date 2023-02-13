using UnityEngine;

public class Tower : Entity, Damageable {
    float health = 1000;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;

    protected override Color? GetHighlightColor() => Color.yellow;
    protected override Color? GetHighlightOutlineColor() => Color.yellow;

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);

        if (health <= 0) {
            Destroy(this);
        }
    }
}
