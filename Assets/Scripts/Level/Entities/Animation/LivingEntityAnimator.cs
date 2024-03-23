using UnityEngine;

public class LivingEntityAnimator : EntityAnimator {
    public LivingEntityAnimator(LivingEntity owner)
        : base(owner) {
    }

    public override void Update() {
        //  Update the players movement animation
        animator.SetBool("isMoving", owner.GetLocomotion().IsMoving());
        //animator.SetFloat("moveSpeedMultiplier", (owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue() / Locomotion.FIXED_MOVEMENT_SPEED), 0.1f, Time.deltaTime);

        //  Update the movement direction for the animation
        Vector3 lookingDirection = owner.GetLocomotion().GetMovementDirection();
        float velocityZ = Vector3.Dot(lookingDirection, owner.transform.forward);
        float velocityX = Vector3.Dot(lookingDirection, owner.transform.right);

        animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);
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
