public class TestAreaTargetAbility : AreaTargetAbility {
    public TestAreaTargetAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() => "TestAreaTargetAbility";
    protected override bool InterruptsMovement() => true;
    protected override int GetManaCost() => 50;
    protected override float GetSpellRadius() => 2;
    protected override float GetRangeRadius() => 10;
}
