using RockUtils.GameEvents;
using UnityEngine;

public class ControllerLocomotion : Locomotion {
    float speed;

    public override MovementType GetMovementType() => MovementType.Controller;

    public ControllerLocomotion(LivingEntity owner)
        : base(owner) {
        InputManager.instance.SetControllerEnabled(true);

        speed = owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue();
    }

    ~ControllerLocomotion() {
        InputManager.instance.SetControllerEnabled(false);
    }

    public override Vector3 GetMovementDirection() {
        return InputManager.getStick(InputManager.ControllerStick.Left);
    }

    public override bool IsMoving() {
        return GetMovementDirection() != Vector3.zero;
    }

    public override void StopMovement() {
        
    }

    protected override void UpdateMovement() {
        base.UpdateMovement();

        owner.transform.position += GetMovementDirection() * speed * Time.deltaTime;
    }

    protected override void UpdateRotation() {
        base.UpdateRotation();

        Vector3 lookDir = GetLookingDirection();
        if (lookDir != Vector3.zero) {
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(lookDir), ROTATION_SPEED * Time.deltaTime);
        }
    }

    protected override Vector3 GetLookingDirection() {
        return InputManager.getStick(InputManager.ControllerStick.Right);
    }

    protected override void SpeedChanged(int param) {
        speed = param / 1000f;
    }
}
