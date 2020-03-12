using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Locomotion : MonoBehaviour {
    //  This values is how fast the player moves per second 6.6 m/s, this value should be tied in with the
    //  animation so we speed up the animation when we're moving faster than normal or slow down when slowed.
    //  TODO: [Rock]: We need a StateMachineBehavior for our locomotion that adjust the animaiton speed based
    //  this value and the current movement speed
    public static readonly float FIXED_MOVEMENT_SPEED = 6.6f;

    static readonly float ROTATION_SPEED = 720f;

    protected NavMeshAgent agent;
    protected Vector3 lookDir;
    protected Vector3 moveDir;

    public void SetDestination(Vector3 destination) {
        agent.SetDestination(destination);
    }

    public bool isMoving() {
        //  We can probably just use Epsilon squared here?
        return moveDir.sqrMagnitude > Mathf.Epsilon * Mathf.Epsilon;
    }

    protected virtual void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update() {
        //  Movement
        moveDir = agent.velocity;
        if (isMoving()) {
            lookDir = moveDir.normalized;

        } else {
            moveDir = Vector3.zero;
        }

        //  Rotation
        if (lookDir.sqrMagnitude > Mathf.Epsilon * Mathf.Epsilon) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDir), ROTATION_SPEED * Time.deltaTime);
        }
    }
}
