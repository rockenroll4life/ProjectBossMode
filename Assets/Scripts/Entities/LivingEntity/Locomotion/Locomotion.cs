using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Locomotion {
    public static readonly float FIXED_MOVEMENT_SPEED = 1.414f;
    protected static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;
    protected NavMeshAgent agent;
    protected Vector3 lookDir;

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

    public virtual void Update() {
        //  Movement
        if (IsMoving()) {
            lookDir = agent.velocity.normalized;
        }

        //  Rotation
        if (lookDir.sqrMagnitude > (Mathf.Epsilon * Mathf.Epsilon)) {
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(lookDir), ROTATION_SPEED * Time.deltaTime);
        }
    }
}
