﻿public class TestAbility : CastAbilityBase {
    protected override string GetName() { return "TestAbility"; }

    protected override float GetCooldownTime() { return 5; }

    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);

        interruptsMovement = true;
    }
}