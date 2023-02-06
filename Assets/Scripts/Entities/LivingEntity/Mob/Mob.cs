using UnityEngine;

public class Mob : LivingEntity {
    GoalSelector goalSelector;
    public override EntityType GetEntityType() { return EntityType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        animator = new LivingEntityAnimator(this);
        targeter = new MobTargeter(this);
        goalSelector = new GoalSelector();
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).SetBaseValue(3f);
    }

    protected virtual void AIStep() {
        goalSelector.Update();
    }

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
