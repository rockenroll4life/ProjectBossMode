using UnityEngine;

public class Mob : LivingEntity {
    public override EntityType GetEntityType() { return EntityType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        //  TODO: [Rock]: Create a MobLocomotion Class if necessary
        locomotion = new Locomotion(this);
        animator = new EntityAnimator(this);
    }

    protected virtual void AIStep() { }

    protected override void UpdateStep() {
        base.UpdateStep();

        //  Handle any AI stuffs or extra handling this entity needs
        AIStep();
    }
}
