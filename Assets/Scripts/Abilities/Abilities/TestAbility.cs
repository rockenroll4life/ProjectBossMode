public class TestAbility : CastAbilityBase {
    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);

        interruptsMovement = true;
    }

    protected override string GetName() { return "TestAbility"; }

    protected override float GetCooldownTime() { return 5; }
}
