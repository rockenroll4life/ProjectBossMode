using RockUtils.GameEvents;
using UnityEngine;

public class PlayerLocomotion : Locomotion {
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
        Vector3? location = owner.GetTargeter().GetTargetedLocation();
        if (location.HasValue) {
            MoveToLocation(location.Value);
        }
    }

    void TargetedEntity(int param) {
        Debug.Log("Targeted an Entity!");
    }
}
