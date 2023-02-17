using System.Collections.Generic;
using UnityEngine;

public abstract class AreaTargetAbility : CastAbilityBase {
    protected AreaTargetAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected abstract float GetSpellRadius();

    protected abstract float GetRangeRadius();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().AreaTarget(SpellIndicators.DEFAULT_COLOR, SpellIndicators.DEFAULT_COLOR, GetSpellRadius(), GetRangeRadius());
    }

    protected override List<Entity> GetEntitiesHitByAbility(int layerMask) {
        List<Entity> hitEntities = new List<Entity>();

        Collider[] hitColliders = Physics.OverlapSphere(getAreaTargetPos(), GetSpellRadius(), layerMask);
        foreach (Collider collider in hitColliders) {
            Entity entity = collider.gameObject.GetComponent<Entity>();
            if (entity) {
                hitEntities.Add(entity);
            }
        }

        return hitEntities;
    }

    Vector3 getAreaTargetPos() {
        Vector3 hitPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LAYER_MASK_GROUND)) {
            float distance = Mathf.Min(GetRangeRadius(), Vector3.Distance(owner.transform.position, hit.point));
            Vector3 dir = (hit.point - owner.transform.position).normalized;
            hitPos = owner.transform.position + (dir * distance);
        }

        return hitPos;
    }
}
