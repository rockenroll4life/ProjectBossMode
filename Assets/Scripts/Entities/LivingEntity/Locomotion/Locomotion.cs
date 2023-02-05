using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Locomotion {
    public static readonly float FIXED_MOVEMENT_SPEED = 1.414f;
    protected static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;
    protected NavMeshAgent agent;

    public Locomotion(LivingEntity owner) {
        this.owner = owner;
        agent = owner.GetComponent<NavMeshAgent>();
        agent.speed = owner.GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).GetValue();
    }

    public void MoveToLocation(Vector3 destination) {
        agent.SetDestination(destination);
    }

    public bool IsMoving() {
        //  We can probably just use Epsilon squared here?
        return agent.velocity.sqrMagnitude > 1f;
    }

    public void StopMovement() {
        agent.ResetPath();
    }

    protected virtual Vector3 GetLookingDirection() {
        return IsMoving() ? agent.velocity.normalized : owner.transform.forward;
    }

    public virtual void Update() {
        //  Rotation
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(GetLookingDirection()), ROTATION_SPEED * Time.deltaTime);
    }
}
