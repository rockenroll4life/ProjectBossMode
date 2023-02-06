using UnityEngine;
using UnityEngine.AI;
using RockUtils.GameEvents;

[RequireComponent(typeof(NavMeshAgent))]
public class Locomotion {
    public static readonly float FIXED_MOVEMENT_SPEED = 1.414f;
    protected static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;
    protected NavMeshAgent agent;

    protected LivingEntity targetedEntity = null;
    protected Vector3? targetedLocation = null;

    public Locomotion(LivingEntity owner) {
        this.owner = owner;
        agent = owner.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = owner.GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).GetValue();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    ~Locomotion() {
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, TargetedWorld);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, TargetedEntity);
    }

    public bool IsMoving() {
        //  We can probably just use Epsilon squared here?
        return agent.velocity.sqrMagnitude > 1f;
    }

    public void MoveToLocation(Vector3 destination) {
        agent.SetDestination(destination);
    }

    public void StopMovement() {
        agent.ResetPath();
    }

    protected virtual Vector3 GetLookingDirection() {
        if (!IsMoving() && targetedEntity) {
            return (targetedEntity.transform.position - owner.transform.position).normalized;
        }

        return IsMoving() ? agent.velocity.normalized : owner.transform.forward;
    }

    public virtual void Update() {
        //  Rotation
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(GetLookingDirection()), ROTATION_SPEED * Time.deltaTime);

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
        targetedEntity = owner.GetTargeter().GetTargetedEntity();

        agent.stoppingDistance = owner.GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
        MoveToLocation(targetedEntity.transform.position);
    }
}
