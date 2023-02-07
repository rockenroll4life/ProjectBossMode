using UnityEngine;

public abstract class Mob : LivingEntity {
    protected GoalSelector goalSelector;
    public override EntityType GetEntityType() { return EntityType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected override void Setup() {
        base.Setup();

        RegisterGoals();
    }

    protected virtual void RegisterGoals() { }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        animator = new LivingEntityAnimator(this);
        targeter = new TargeterBase(this);
        goalSelector = new GoalSelector();
    }

    protected virtual void AIStep() {
        goalSelector.Update();
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        AIStep();
    }
}
