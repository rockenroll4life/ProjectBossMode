public class TestMob : Mob {
    protected override void RegisterGoals() {
        base.RegisterGoals();

        goalSelector.AddGoal(4, new PatrolGoal(this));
        goalSelector.AddGoal(2, new AttackAggressorGoal(this));
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).SetBaseValue(3f);
    }
}
