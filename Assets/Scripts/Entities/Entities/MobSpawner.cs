using UnityEngine;

public class MobSpawner : Entity, Damageable {
    public GameObject MobPrefab;

    float health = 25;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.Destructable;

    protected override Color? GetHighlightColor() => Color.black;
    protected override Color? GetHighlightOutlineColor() => Color.black;

    protected override void Setup() {
        base.Setup();

        Debug.Assert(MobPrefab, "NO MOB SET ON MOB SPAWNER!");

        Instantiate(MobPrefab, transform.position, transform.rotation);
    }

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        Debug.Log(name + " Health: " + health);

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Mob");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
