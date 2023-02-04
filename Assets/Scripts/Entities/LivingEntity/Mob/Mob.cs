using UnityEngine;

public class Mob : LivingEntity {
    public override EntityType GetEntityType() { return EntityType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        //  TODO: [Rock]: Create a MobLocomotion Class if necessary
        locomotion = new MobLocomotion(this);
        animator = new LivingEntityAnimator(this);
        targeter = new MobTargeter(this);
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).SetBaseValue(3f);
    }

    protected virtual void AIStep() { }

    protected override void UpdateStep() {
        base.UpdateStep();

        //  Handle any AI stuffs or extra handling this entity needs
        AIStep();
    }

    public override void Hurt(Entity damager, float damage) {
        base.Hurt(damager, damage);

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
