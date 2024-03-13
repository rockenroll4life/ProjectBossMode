using System.Collections.Generic;
using UnityEngine;

public class TestAOEAbility : AOEAbilityBase {
    public TestAOEAbility(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected override float GetSpellRadius() => 6;

    protected override void CastAbility() {
        base.CastAbility();

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}
