public class TestAbility : ConeAbilityBase {
    public TestAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
    }

    protected override string GetName() =>"TestAbility";
    protected override float GetSpellAngle() => 90;
    protected override float GetSpellRadius() => 7;
    public override Ability.ID GetID() => Ability.ID.FireBreath;
}
