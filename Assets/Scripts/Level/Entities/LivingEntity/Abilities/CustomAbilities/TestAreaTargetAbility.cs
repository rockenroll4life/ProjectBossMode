using System.Collections.Generic;
using UnityEngine;

public class TestAreaTargetAbility : AreaTargetAbilityBase {
    public TestAreaTargetAbility(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected override bool InterruptsMovement() => true;
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
