using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 5;
    public float collisionDistance = 1f;

    LivingEntity owner;
    LivingEntity target;
    float damage;

    public void Setup(LivingEntity owner, LivingEntity target, float damage) {
        this.owner = owner;
        this.target = target;
        this.damage = damage;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + Vector3.up, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= collisionDistance) {
            target.Hurt(owner, damage);
            
            Destroy(gameObject);
        }
    }
}
