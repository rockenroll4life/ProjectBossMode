public class TestNoStopAbility : ToggleAbilityBase {
    public TestNoStopAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
    }

    protected override string GetName() => "TestNoStopAbility";
    public override Ability.ID GetID() => Ability.ID.BurningPassion;
}
