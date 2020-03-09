using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    enum MovementState {
        Moving,   //  For now this encapsulates Idle / Walking / Running (Walking is just moving with a speed of 0!)
        Rolling,
    }

    public float movementSpeed = 5f;

    Camera cam;
    Animator animator;
    Rigidbody physicsbody;
    InputManager input;

    MovementState movementState;

    Vector3 camOffset;

    //  TODO: Remove this?
    Vector3 currentMovingVelocity = Vector3.forward;

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

        UpdateState();

        UpdateAnimations();
    }

    private void LateUpdate() {
        cam.transform.position = transform.position + camOffset;
    }

    void UpdateState() {
        if (movementState == MovementState.Moving) {
            //  Check for movement Input

            //  Movement / Rotation
            currentMovingVelocity = input.getStick(InputManager.Stick.Left);
            if (currentMovingVelocity.sqrMagnitude > 0.1 * 0.1) {
                //  Move the player
                physicsbody.MovePosition(transform.position + (currentMovingVelocity.normalized * movementSpeed * Time.deltaTime));

                //  Rotate the player
                transform.rotation = Quaternion.LookRotation(currentMovingVelocity);
            }

            //  Rolling
            if (input.isDown(InputManager.Buttons.A)) {
                movementState = MovementState.Rolling;
            }
        }

        if (movementState ==  MovementState.Rolling) {
            //  TODO: Turn this move into a burst (explosive) move, it's a dive roll...act awesome!

            //  Since we're rolling we can't change directions, so just roll in the last direction we were looking
            physicsbody.MovePosition(transform.position + (transform.forward * movementSpeed * Time.deltaTime));
        }
    }

    void UpdateAnimations() {
        //  Update the players movement animation
        float speedPercent = (Mathf.Abs(currentMovingVelocity.x) + Mathf.Abs(currentMovingVelocity.z)) > Mathf.Epsilon ? 1 : 0;
        animator.SetFloat("speedPercent", speedPercent , 0.1f, Time.deltaTime);

        //   Update animation controller rolling state
        animator.SetBool("isRolling", movementState == MovementState.Rolling);
    }

    public void StartRoll() {
        Debug.Log("Roll Started");
    }

    public void FinishRoll() {
        Debug.Log("Roll Finished");

        //  We finished our roll animation so transition back to our move state
        movementState = MovementState.Moving;
    }
}
