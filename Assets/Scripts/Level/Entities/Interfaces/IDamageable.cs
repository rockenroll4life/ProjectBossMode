public interface IDamageable {
    public Entity GetEntity();

    public void DealDamage(Entity damager, float damage);
}
