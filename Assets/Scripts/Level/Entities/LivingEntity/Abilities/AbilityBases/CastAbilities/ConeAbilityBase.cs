using System.Collections.Generic;
using UnityEngine;

public abstract class ConeAbilityBase : CastAbilityBase {
    public ConeAbilityBase(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected abstract float GetSpellRadius();

    protected abstract float GetSpellAngle();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().Cone(SpellIndicators.DEFAULT_COLOR, GetSpellRadius(), GetSpellAngle());
    }

    protected override List<Entity> GetEntitiesHitByAbility(int layerMask) {
        List<Entity> hitEntities = new List<Entity>();
        Vector3 coneDirection = GetConeDirection();
        float halfAngle = GetSpellAngle() / 2f;

        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, GetSpellRadius(), layerMask);
        foreach (Collider collider in hitColliders) {
            Entity entity = collider.gameObject.GetComponent<Entity>();
            if (entity) {
                //  Get the direction to the entity
                Vector3 directionToEntity = (entity.transform.position - owner.transform.position).normalized;

                //  Then check to see if the enemy is within the angle of that direction
                float angleBetween = Vector3.Angle(coneDirection, directionToEntity);
                if (angleBetween <= halfAngle) {
                    hitEntities.Add(entity);
                }
            }
        }

        return hitEntities;
    }

    Vector3 GetConeDirection() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LAYER_MASK_GROUND)) {
            return (hit.point - owner.transform.position).normalized;
        }

        return Vector3.zero;
    }
}
