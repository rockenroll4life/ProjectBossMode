using UnityEngine;

public class LivingEntityAnimator : EntityAnimator {
    static readonly float FIXED_ANIMATION_SPEED = 3.5f;

    float moveSpeedMultiplier = 1f;

    public LivingEntityAnimator(LivingEntity owner)
        : base(owner) {

        owner.GetAttributes().RegisterListener(AttributeTypes.MovementSpeed, ValueChanged);
        moveSpeedMultiplier = (owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue() / FIXED_ANIMATION_SPEED);
    }

    public override void Update() {
        //  Update the players movement animation
        animator.SetBool("isMoving", owner.GetLocomotion().IsMoving());
        animator.SetFloat("moveSpeedMultiplier", moveSpeedMultiplier, 0.1f, Time.deltaTime);

        //  Update the movement direction for the animation
        Vector3 lookingDirection = owner.GetLocomotion().GetMovementDirection();
        float velocityZ = Vector3.Dot(lookingDirection, owner.transform.forward);
        float velocityX = Vector3.Dot(lookingDirection, owner.transform.right);

        animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);
    }

    void ValueChanged(int param) {
        float value = (param / 1000f);
        moveSpeedMultiplier = (value / FIXED_ANIMATION_SPEED);
    }
}
