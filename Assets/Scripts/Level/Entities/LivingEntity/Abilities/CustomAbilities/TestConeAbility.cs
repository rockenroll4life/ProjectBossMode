using System.Collections.Generic;
using UnityEngine;

public class TestConeAbility : ConeAbilityBase {
    public TestConeAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
        cost = new ResourceCost(owner, EntityDataType.Mana, 20);
    }

    protected override string GetName() =>"TestConeAbility";
    public override Ability.ID GetID() => Ability.ID.FireBreath;
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
