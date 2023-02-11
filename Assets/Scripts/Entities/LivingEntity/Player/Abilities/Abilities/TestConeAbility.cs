public class TestConeAbility : ConeAbility {
    public TestConeAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() =>"TestConeAbility";
    protected override bool InterruptsMovement() => true;
    protected override int GetManaCost() => 20;
    protected override float GetSpellAngle() => 180;
    protected override float GetSpellRadius() => 5;
}
