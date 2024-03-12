using System.Collections.Generic;
using UnityEngine;

public class TestAreaTargetAbility : AreaTargetAbilityBase {
    public TestAreaTargetAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
        cost = new ResourceCost(owner, EntityDataType.Mana, 50);
    }

    protected override string GetName() => "TestAreaTargetAbility";
    protected override bool InterruptsMovement() => true;
    protected override float GetSpellRadius() => 2;
    protected override float GetRangeRadius() => 10;
    public override Ability.ID GetID() => Ability.ID.Meteor;

    protected override void CastAbility() {
        base.CastAbility();

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}
