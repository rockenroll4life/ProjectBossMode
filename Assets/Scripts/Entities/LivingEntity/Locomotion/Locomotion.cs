using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Locomotion {
    //  This values is how fast the player moves per second 6.6 m/s, this value should be tied in with the
    //  animation so we speed up the animation when we're moving faster than normal or slow down when slowed.
    //  TODO: [Rock]: We need a StateMachineBehavior for our locomotion that adjust the animation speed based
    //  this value and the current movement speed
    //public static readonly float FIXED_MOVEMENT_SPEED = 6.6f;
    public static readonly float FIXED_MOVEMENT_SPEED = 4.95f;
    static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;
    protected NavMeshAgent agent;
    protected Vector3 lookDir;

    public Locomotion(LivingEntity owner) {
        this.owner = owner;
        agent = owner.GetComponent<NavMeshAgent>();
    }

    public void MoveToLocation(Vector3 destination) {
        agent.SetDestination(destination);
    }

    public bool IsMoving() {
        //  We can probably just use Epsilon squared here?
        return agent.velocity.sqrMagnitude > (Mathf.Epsilon * Mathf.Epsilon);
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
