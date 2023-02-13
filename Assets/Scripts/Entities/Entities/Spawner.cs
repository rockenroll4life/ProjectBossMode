using UnityEngine;

public class Spawner : Entity, Damageable {
    float health = 1000;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;

    protected override Color? GetHighlightColor() => Color.black;
    protected override Color? GetHighlightOutlineColor() => Color.black;

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);

        if (health <= 0) {
            Destroy(this);
        }
    }
}
