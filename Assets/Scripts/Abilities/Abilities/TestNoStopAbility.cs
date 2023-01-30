public class TestNoStopAbility : ToggleAbilityBase {
    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);
    }

    protected override string GetName() { return "TestNoStopAbility"; }
}
