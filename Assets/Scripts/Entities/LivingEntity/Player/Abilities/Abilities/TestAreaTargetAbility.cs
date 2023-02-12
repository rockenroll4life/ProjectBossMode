using System.Collections.Generic;
using UnityEngine;

public class TestAreaTargetAbility : AreaTargetAbility {
    public TestAreaTargetAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() => "TestAreaTargetAbility";
    protected override bool InterruptsMovement() => true;
    public override int GetManaCost() => 50;
    protected override float GetSpellRadius() => 2;
    protected override float GetRangeRadius() => 10;

    protected override void CastAbility() {
        base.CastAbility();

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}
