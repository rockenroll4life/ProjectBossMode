using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {
    enum MovementState {
        Moving,   //  For now this encapsulates Idle / Running
    }

    //  This values is how fast the player moves per second 6.6 m/s, this value should be tied in with the
    //  animation so we speed up the animation when we're moving faster than normal or slow down when slowed.
    //  TODO: [Rock]: We need a StateMachineBehavior for our locomotion that adjust the animaiton speed based
    //  this value and the current movement speed
    static readonly float FIXED_MOVEMENT_SPEED = 6.6f;

    //  Public Variables
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public GameObject testSwordForAttaching;

    //  The rest of the mess...

    //  TODO: [Rock]: Cleanup the order of our variables...too lazy and will mess it up while I'm starting to dev this
    readonly float rotationSpeed = 720f;

    bool hasWeaponLeft = false;
    bool hasWeaponRight = false;

    Camera cam;
    Animator animator;
    Rigidbody physicsbody;
    NavMeshAgent agent;
    InputManager input;

    MovementState movementState;
    //  TODO: [Rock]: Move to stats class that supports modifications of the base value
    readonly float movementSpeed = 13.2f;

    Vector3 movingDir = Vector3.forward;
    bool moving = false;

    void Start() {
        cam = Camera.main;

        animator = GetComponentInChildren<Animator>();
        physicsbody = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        input = new InputManager(0);

        EventManager.StartListening((int) GameEvents.Mouse_LeftClick, MouseLeftClick);

        //  TODO: [Rock]: We should have an equipment class that handles setting this when we change our equipment
        animator.SetBool("hasWeapon_Right", false);
        animator.SetBool("hasWeapon_Left", false);

        /*GameObject rightWeapon = Instantiate(testSwordForAttaching, handAttachRight.transform, true);
        rightWeapon.transform.localPosition = Vector3.zero;
        //  TODO: [Rock]: Figure out the stupid attachment bone in the model so we don't need to rotate the model
        //  For now just leave it as this so we can make some actual progress with gameplay :(
        rightWeapon.transform.localRotation = Quaternion.Euler(0, 0, 90);

        GameObject leftWeapon = Instantiate(testSwordForAttaching, handAttachLeft.transform, true);
        leftWeapon.transform.localPosition = Vector3.zero;
        leftWeapon.transform.localRotation = Quaternion.Euler(0, 0, 90);*/
    }

    private void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
    }


    void Update() {
         //  Update our input manager to track inputs
        input.update();

        CheckInput();

        UpdateState();

        UpdateAnimations();
    }

    void MouseLeftClick() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {
            agent.SetDestination(hit.point);

            //  
            //movingDir = hit.point - transform.position;
        }
    }

    void MouseRightClick() {
        Debug.Log("Right Clicked!");
    }

    void CheckInput() {
        //  Debug, can be removed
        if (input.wasPressed(InputManager.ControllerButtons.Right_Bumper)) {
            hasWeaponRight = !hasWeaponRight;
            animator.SetBool("hasWeapon_Right", hasWeaponRight);
        }

        if (input.wasPressed(InputManager.ControllerButtons.Left_Bumper)) {
            hasWeaponLeft = !hasWeaponLeft;
            animator.SetBool("hasWeapon_Left", hasWeaponLeft);
        }
    }

    void UpdateState() {
        if (movementState == MovementState.Moving) {
            //  Check for movement Input

            //  Movement / Rotation
            Vector3 moveVelocity = agent.velocity;//input.getStick(InputManager.Stick.Left);
            if (moveVelocity.sqrMagnitude > 0.1 * 0.1) {
                movingDir = moveVelocity.normalized;
                moving = true;

                //  Move the player
                //  TODO: [Rock]: Investigate if we should be still be manually moving our player along a "path" instead of letting the agent
                //  automatically move it itself
                //physicsbody.MovePosition(transform.position + (movingDir * movementSpeed * Time.deltaTime));
            } else {
                moving = false;
            }

            //  Rotate the player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movingDir), rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateAnimations() {
        //  Update the players movement animation
        animator.SetBool("isMoving", moving);
        animator.SetFloat("moveSpeedMultiplier", (movementSpeed / FIXED_MOVEMENT_SPEED) , 0.1f, Time.deltaTime);
    }
}
