using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed = 5f;
    public AnimationCurve moveSpeedCurve;

    Camera cam;
    Animator animator;
    Rigidbody rigidbody;

    InputManager input;

    Vector3 camOffset;

    //  TODO: Remove this?
    Vector3 currentMovingVelocity;
    bool pressed = false;

    void Start(){
        cam = Camera.main;

        camOffset = cam.transform.position - transform.position;

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        input = new InputManager(0);
    }

    void Update() {
        //  Update our input manager to track inputs
        input.update();

        CheckInput();
        UpdateAnimations();
    }

    private void LateUpdate() {
        cam.transform.position = transform.position + camOffset;
    }

    void CheckInput() {
        //  TODO: We need to add movement states to support things such as rolling

        //  Movement
        currentMovingVelocity = input.getStick(InputManager.Stick.Left);
        if (currentMovingVelocity.sqrMagnitude > 0.1 * 0.1) {
            float val = Mathf.Max(Mathf.Abs(currentMovingVelocity.x), Mathf.Abs(currentMovingVelocity.z));
            float actualSpeed = moveSpeedCurve.Evaluate(val);

            //  Move the player
            rigidbody.MovePosition(transform.position + (currentMovingVelocity.normalized * (actualSpeed * movementSpeed) * Time.deltaTime));
            
            //  Rotate the player
            transform.rotation = Quaternion.LookRotation(currentMovingVelocity);
        }

        pressed = input.isDown(InputManager.Buttons.A);
    }

    void UpdateAnimations() {
        //  Update the players movement animation
        float speedPercent = Mathf.Max(Mathf.Abs(currentMovingVelocity.x), Mathf.Abs(currentMovingVelocity.z)) / 1f;
        animator.SetFloat("speedPercent", speedPercent, 0.1f, Time.deltaTime);
    }
}
