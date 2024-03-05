public class TestMob : Mob {
    protected override void RegisterGoals() {
        base.RegisterGoals();

        goalSelector.AddGoal(6, new PatrolGoal(this));
        goalSelector.AddGoal(4, new AttackTower(this));
        goalSelector.AddGoal(2, new AttackAggressorGoal(this));
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        GetAttribute(AttributeTypes.MovementSpeed).SetBaseValue(3f);
    }
}
