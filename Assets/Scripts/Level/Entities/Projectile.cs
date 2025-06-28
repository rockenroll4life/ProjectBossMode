using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 5;
    public float collisionDistance = 0.5f;

    LivingEntity owner;
    IDamageable target;
    float damage;

    public void Setup(LivingEntity owner, IDamageable target, float damage) {
        this.owner = owner;
        this.target = target;
        this.damage = damage;
    }

    void Update() {
        if (target == null || target.GetEntity() == null) {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.GetEntity().transform.position + Vector3.up, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.GetEntity().transform.position);
        if (distance <= collisionDistance) {
            target.Hurt(owner, damage);
            
            Destroy(gameObject);
        }
    }
}
