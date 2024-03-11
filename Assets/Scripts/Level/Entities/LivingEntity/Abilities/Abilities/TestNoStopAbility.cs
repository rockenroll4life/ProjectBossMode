public class TestNoStopAbility : ToggleAbilityBase {
    public TestNoStopAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
    }

    protected override string GetName() => "TestNoStopAbility";
}
