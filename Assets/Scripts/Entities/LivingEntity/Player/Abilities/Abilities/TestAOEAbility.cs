public class TestAOEAbility : AOEAbility {
    public TestAOEAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() => "TestAOEAbility";
    protected override int GetManaCost() => 30;
    protected override float GetSpellRadius() => 6;
}
