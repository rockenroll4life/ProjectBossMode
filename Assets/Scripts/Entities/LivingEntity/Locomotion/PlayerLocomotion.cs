using RockUtils.GameEvents;
using UnityEngine;

public class PlayerLocomotion : Locomotion {
    LivingEntity targetedEntity = null;

    public PlayerLocomotion(LivingEntity owner)
        : base(owner) {
        agent.updateRotation = false;

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    ~PlayerLocomotion() {
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    void TargetedWorld(int param) {
        targetedEntity = null;
        
        Vector3? location = owner.GetTargeter().GetTargetedLocation();
        if (location.HasValue) {
            agent.stoppingDistance = Mathf.Epsilon;
            MoveToLocation(location.Value);
        }
    }

    void TargetedEntity(int param) {
        targetedEntity = owner.GetTargeter().GetTargetedEntity();

        agent.stoppingDistance = owner.GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
        MoveToLocation(targetedEntity.transform.position);
    }

    public override void Update() {
        base.Update();

        if (targetedEntity) {
            MoveToLocation(targetedEntity.transform.position);

            Vector3 dir = (targetedEntity.transform.position - owner.transform.position).normalized;
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(dir), ROTATION_SPEED * Time.deltaTime);
        }
    }
}
