using UnityEngine;
using RockUtils.GameEvents;

public class LivingEntityLocomotion : LocomotionBase {
    protected LivingEntity targetedEntity = null;

    public LivingEntityLocomotion(LivingEntity owner)
        : base(owner) {
        agent.updateRotation = false;

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    ~LivingEntityLocomotion() {
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    protected virtual void TargetedWorld(int param) {
        targetedEntity = null;

        Vector3? location = owner.GetTargeter().GetTargetedLocation();
        if (location.HasValue) {
            agent.stoppingDistance = Mathf.Epsilon;
            MoveToLocation(location.Value);
        }
    }
    protected virtual void TargetedEntity(int param) {
        targetedEntity = owner.GetTargeter().GetTargetedEntity();

        agent.stoppingDistance = owner.GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
        MoveToLocation(targetedEntity.transform.position);
    }

    protected override Vector3 GetLookingDirection() {
        if (!IsMoving() && targetedEntity) {
            return (targetedEntity.transform.position - owner.transform.position).normalized;
        }

        return base.GetLookingDirection();
    }
}
