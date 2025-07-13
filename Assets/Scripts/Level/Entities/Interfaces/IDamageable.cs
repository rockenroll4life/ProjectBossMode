public interface IDamageable {
    public Entity GetEntity();

    public void Hurt(Entity damager, float damage);

    //  TODO: [Rock]: Implement OnDeath Function
}
