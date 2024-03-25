using System.Collections.Generic;
using UnityEngine;

public class TestConeAbility : ConeAbilityBase {
    static readonly string EFFECT_LOCATION = $"{ABILITY_RESOURCE_LOCATION}/VFX_FireBreath";
    readonly GameObject effectPrefab;

    public TestConeAbility(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
        effectPrefab = Resources.Load<GameObject>(EFFECT_LOCATION);
    }

    protected override bool InterruptsMovement() => true;
    protected override float GetSpellAngle() => 45;
    protected override float GetSpellRadius() => 4.5f;

    protected override void CastAbility() {
        base.CastAbility();

        PlayEffect(effectPrefab, Vector3.up * 0.5f);

        List<Entity> hitEntities = GetEntitiesHitByAbility(LAYER_MASK_MOB);
        Debug.Log("Entities hit: " + hitEntities.Count);
        foreach (Entity entity in hitEntities) {
            Debug.Log(entity.name);
        }
    }
}
