using UnityEngine;
using RockUtils.GameEvents;

public class LivingEntityTargeter : TargeterBase {
    protected LivingEntity targetedEntity = null;
    protected Vector3? targetedLocation = null;

    public LivingEntityTargeter(LivingEntity owner)
        : base(owner) {
    }

    public override LivingEntity GetTargetedEntity() {
        return targetedEntity;
    }

    public override Vector3? GetTargetedLocation() {
        return targetedLocation;
    }

    public override void SetTargetedEntity(LivingEntity entity) {
        targetedEntity = entity;

        if (entity) {
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_Entity);
        }
    }

    public override void SetTargetedLocation(Vector3? location) {
        targetedLocation = location;

        if (location.HasValue) {
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
        }
    }
}
