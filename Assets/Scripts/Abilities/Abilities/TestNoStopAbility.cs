public class TestNoStopAbility : ToggleAbilityBase {
    public override void Setup(LivingEntity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);
    }

    protected override string GetName() { return "TestNoStopAbility"; }
}
