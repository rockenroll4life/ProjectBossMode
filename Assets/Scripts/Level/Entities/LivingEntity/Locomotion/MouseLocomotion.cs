using UnityEngine;
using UnityEngine.AI;
using RockUtils.GameEvents;

public class MouseLocomotion : Locomotion {
    private readonly NavMeshAgent agent;

    private Entity targetedEntity = null;
    private Vector3? targetedLocation = null;
    private float attackRange;

    public override MovementType GetMovementType() => MovementType.Mouse;

    public override Vector3 GetMovementDirection() => IsMoving() ? agent.velocity.normalized : Vector3.zero;

    public MouseLocomotion(LivingEntity owner)
        : base(owner) {
        attackRange = owner.GetAttribute(AttributeTypes.AttackRange).GetValue();
        owner.GetAttributes().RegisterListener(AttributeTypes.AttackRange, AttackRangeChanged);

        agent = owner.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_World, TargetedWorld);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_Entity, TargetedEntity);
    }

    ~MouseLocomotion() {
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_World, TargetedWorld);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_Entity, TargetedEntity);
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

    protected override void UpdateMovement() {
        if (targetedEntity) {
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

        agent.stoppingDistance = owner.GetAttribute(AttributeTypes.AttackRange).GetValue();
        MoveToLocation(targetedEntity.transform.position);
    }

    protected override void SpeedChanged(int param) {
        agent.speed = param / 1000f;
    }

    void AttackRangeChanged(int param) {
        attackRange = param / 1000f;
    }
}
