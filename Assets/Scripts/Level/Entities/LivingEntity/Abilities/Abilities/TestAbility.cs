public class TestAbility : ConeAbility {
    public TestAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
        cost = new ResourceCost(owner, EntityDataType.Mana, 10);
    }

    protected override string GetName() =>"TestAbility";
    protected override float GetSpellAngle() => 90;
    protected override float GetSpellRadius() => 7;
}
