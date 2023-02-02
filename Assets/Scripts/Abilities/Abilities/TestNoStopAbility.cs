public class TestNoStopAbility : ToggleAbilityBase {
    public TestNoStopAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() { return "TestNoStopAbility"; }
}
