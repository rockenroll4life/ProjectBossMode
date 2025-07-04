using System.Collections.Generic;
using UnityEngine;

public abstract class AOEAbilityBase : CastAbilityBase {
    public AOEAbilityBase(LivingEntity owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected abstract float GetSpellRadius();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().AOE(SpellIndicators.DEFAULT_COLOR, GetSpellRadius());
    }

    protected override List<Entity> GetEntitiesHitByAbility(int layerMask) {
        List<Entity> hitEntities = new List<Entity>();
        
        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, GetSpellRadius(), layerMask);
        foreach (Collider collider in hitColliders) {
            Entity entity = collider.gameObject.GetComponent<Entity>();
            if (entity) {
                hitEntities.Add(entity);
            }
        }

        return hitEntities;
    }
}
