using UnityEngine;

public class Player : MonoBehaviour {
    //  Public Variables
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public GameObject testSwordForAttaching;

    Animator animator;
    PlayerLocomotion locomotion;

    //  TODO: [Rock]: Move to stats class that supports modifications of the base value
    readonly float movementSpeed = 13.2f;

    void Start() {
        //  Event Listeners
        EventManager.StartListening((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
        
        animator = GetComponentInChildren<Animator>();
        locomotion = gameObject.AddComponent<PlayerLocomotion>();
    }

    private void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
    }

    void Update() {
        //  Update the players movement animation
        animator.SetBool("isMoving", locomotion.isMoving());
        animator.SetFloat("moveSpeedMultiplier", (movementSpeed / Locomotion.FIXED_MOVEMENT_SPEED), 0.1f, Time.deltaTime);
    }

    void MouseLeftClick() {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.SetDestination(hit.point);
        }
    }

    void TEST_ATTACHWEAPONS() {
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
}
