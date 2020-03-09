using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    enum MovementState {
        Moving,   //  For now this encapsulates Idle / Walking / Running (Walking is just moving with a speed of 0!)
        Rolling,
    }

    //  TODO: [Rock]: Cleanup the order of our variables...too lazy and will mess it up while I'm starting to dev this
    readonly float movementSpeed = 25f;
    readonly float rollSpeed = 40f;
    readonly float rotationSpeed = 720f;

    Camera cam;
    Animator animator;
    Rigidbody physicsbody;
    InputManager input;

    MovementState movementState;

    Vector3 camOffset;

    Vector3 movingDir = Vector3.forward;
    bool moving = false;

    void Start() {
        cam = Camera.main;

        camOffset = cam.transform.position - transform.position;

        animator = GetComponentInChildren<Animator>();
        physicsbody = GetComponent<Rigidbody>();

        input = new InputManager(0);
    }

    void Update() {
        //  Update our input manager to track inputs
        input.update();

        CheckInput();

        UpdateState();

        UpdateAnimations();
    }

    private void LateUpdate() {
        cam.transform.position = transform.position + camOffset;
    }

    void CheckInput() {
        if (movementState == MovementState.Moving) {
            //  Rolling
            if (input.isDown(InputManager.Buttons.A)) {
                movementState = MovementState.Rolling;

                //  Because we're going to do a roll, just snap the players rotation instead of doing the lerp like normal
                transform.rotation = Quaternion.LookRotation(movingDir);
            }
        }
    }

    void UpdateState() {
        if (movementState == MovementState.Moving) {
            //  Check for movement Input

            //  Movement / Rotation
            Vector3 moveVelocity = input.getStick(InputManager.Stick.Left);
            if (moveVelocity.sqrMagnitude > 0.1 * 0.1) {
                movingDir = moveVelocity.normalized;
                moving = true;

                //  Move the player
                physicsbody.MovePosition(transform.position + (movingDir * movementSpeed * Time.deltaTime));
            } else {
                moving = false;
            }

            //  Rotate the player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movingDir), rotationSpeed * Time.deltaTime);
        }

        if (movementState == MovementState.Rolling) {
            //  Since we're rolling we can't change directions, so just roll in the last direction we were looking
            physicsbody.MovePosition(transform.position + (transform.forward * rollSpeed * Time.deltaTime));

            //  TOOD: [Rock]: Decide if we want to allow for turning while we roll? Leaning towards no, but worth a thought
        }
    }

    void UpdateAnimations() {
        //  Update the players movement animation
        animator.SetFloat("speedPercent", moving ? 1 : 0 , 0.1f, Time.deltaTime);

        //   Update animation controller rolling state
        animator.SetBool("isRolling", movementState == MovementState.Rolling);
    }

    public void OnStartRoll_AnimaitonEvent() {
        //  TODO: [Rock]: Do we actually need this?
    }

    public void OnFinishRoll_AnimationEvent() {
        //  We finished our roll animation so transition back to our move state
        movementState = MovementState.Moving;
    }
}
