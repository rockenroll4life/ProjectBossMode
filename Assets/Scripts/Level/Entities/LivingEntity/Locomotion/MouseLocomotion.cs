using UnityEngine;
using UnityEngine.AI;
using RockUtils.GameEvents;

public class MouseLocomotion : Locomotion {
    private NavMeshAgent agent;

    private Entity targetedEntity = null;
    private Vector3? targetedLocation = null;

    public override MovementType GetMovementType() => MovementType.Mouse;

    public MouseLocomotion(LivingEntity owner)
        : base(owner) {
        agent = owner.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = owner.GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).GetValue();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    ~MouseLocomotion() {
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    public override bool IsMoving() {
        //  We can probably just use Epsilon squared here?
        return agent.velocity.sqrMagnitude > 1f;
    }

    void MoveToLocation(Vector3 destination) {
        agent.SetDestination(destination);
    }

    public override void StopMovement() {
        agent.ResetPath();
    }

    protected override Vector3 GetLookingDirection() {
        if (!IsMoving() && targetedEntity) {
            return (targetedEntity.transform.position - owner.transform.position).normalized;
        }

        return IsMoving() ? agent.velocity.normalized : owner.transform.forward;
    }

    protected override void UpdateRotation() {
        base.UpdateRotation();

        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(GetLookingDirection()), ROTATION_SPEED * Time.deltaTime);
    }

    protected override void UpdateMovement() {
        base.UpdateMovement();

        //  TODO: [Rock]: Think of a way we can optimize this instead of it checking every frame
        if (targetedEntity) {
            float attackRange = owner.GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
            if ((targetedEntity.transform.position - owner.transform.position).sqrMagnitude > (attackRange * attackRange)) {
                MoveToLocation(targetedEntity.transform.position);
            }
        }
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
        targetedEntity = owner.GetTargeter().GetTargetedEntity().GetEntity();

        agent.stoppingDistance = owner.GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
        MoveToLocation(targetedEntity.transform.position);
    }
}