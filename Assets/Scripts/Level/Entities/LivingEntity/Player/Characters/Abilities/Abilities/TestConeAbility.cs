﻿using System.Collections.Generic;
using UnityEngine;

public class TestConeAbility : ConeAbility {
    public TestConeAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() =>"TestConeAbility";
    protected override bool InterruptsMovement() => true;
    public override int GetManaCost() => 20;
    protected override float GetSpellAngle() => 180;
    protected override float GetSpellRadius() => 5;

    protected override void CastAbility() {
        base.CastAbility();

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}