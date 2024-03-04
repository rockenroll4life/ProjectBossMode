using System.Collections.Generic;
using UnityEngine;

public class TestConeAbility : ConeAbility {
    public TestConeAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
        cost = new ResourceCost(ResourceType.Mana, 20);
    }

    protected override string GetName() =>"TestConeAbility";
    protected override bool InterruptsMovement() => true;
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
