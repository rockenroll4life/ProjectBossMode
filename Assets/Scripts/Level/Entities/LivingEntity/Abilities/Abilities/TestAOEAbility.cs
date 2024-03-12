using System.Collections.Generic;
using UnityEngine;

public class TestAOEAbility : AOEAbilityBase {
    public TestAOEAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
        cost = new ResourceCost(owner, EntityDataType.Mana, 30);
    }

    protected override string GetName() => "TestAOEAbility";
    protected override float GetSpellRadius() => 6;
    public override Ability.ID GetID() => Ability.ID.RingOfFire;

    protected override void CastAbility() {
        base.CastAbility();

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}
