using UnityEngine;

//  TODO: [Rock]: Make Mob an abstract class
public class Mob : LivingEntity {
    GoalSelector goalSelector;
    public override EntityType GetEntityType() { return EntityType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected override void Setup() {
        base.Setup();

        RegisterGoals();
    }

    //  TODO: [Rock]: Make RegisterGoals an abstract function
    protected virtual void RegisterGoals() {
        goalSelector.AddGoal(4, new PatrolGoal(this));
        goalSelector.AddGoal(2, new AttackAggressorGoal(this));
    }

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

    public override void Hurt(LivingEntity damager, float damage) {
        base.Hurt(damager, damage);

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
