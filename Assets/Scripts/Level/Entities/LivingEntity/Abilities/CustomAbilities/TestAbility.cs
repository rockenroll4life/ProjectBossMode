public class TestAbility : ConeAbilityBase {
    public TestAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
        cost = new ResourceCost(owner, EntityDataType.Mana, 10);
    }

    protected override string GetName() =>"TestAbility";
    protected override float GetSpellAngle() => 90;
    protected override float GetSpellRadius() => 7;
    public override Ability.ID GetID() => Ability.ID.FireBreath;
}
