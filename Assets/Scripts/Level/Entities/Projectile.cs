using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 5;
    public float collisionDistance = 0.5f;

    private LivingEntity owner;
    private IDamageable target;
    private float damage;

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

        //  NOTE: [Rock]: We should probably be checking against the collision box instead of it just being "close enough"
        float distance = Vector3.Distance(transform.position, target.GetEntity().transform.position);
        //  TEMP: [Rock]: We're going to default this to 1 so it works for the time being...
        if (distance <= 1.0f/*collisionDistance*/) {
            target.Hurt(owner, damage);
            
            Destroy(gameObject);
        }
    }
}
