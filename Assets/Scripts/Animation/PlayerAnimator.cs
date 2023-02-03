using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : EntityAnimator {
    protected override void UpdateAnimations() {
        //  Update the players movement animation
        animator.SetBool("isMoving", owner.GetLocomotion().IsMoving());
        animator.SetFloat("moveSpeedMultiplier", (owner.GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).GetValue()  / Locomotion.FIXED_MOVEMENT_SPEED), 0.1f, Time.deltaTime);
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
