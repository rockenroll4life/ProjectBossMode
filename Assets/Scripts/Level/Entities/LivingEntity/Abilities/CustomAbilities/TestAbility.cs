public class TestAbility : ConeAbilityBase {
    public TestAbility(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected override float GetSpellAngle() => 90;
    protected override float GetSpellRadius() => 7;
}
