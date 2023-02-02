public class TestAbility : CastAbilityBase {
    public TestAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
        interruptsMovement = true;
    }

    protected override string GetName() { return "TestAbility"; }
}
