﻿public class TestAbility : ConeAbility {
    public TestAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() =>"TestAbility";
    protected override float GetSpellAngle() => 90;
    protected override float GetSpellRadius() => 7;
}