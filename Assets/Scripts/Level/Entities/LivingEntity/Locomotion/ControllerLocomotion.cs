using RockUtils.GameEvents;
using UnityEngine;

public class ControllerLocomotion : Locomotion {
    public override MovementType GetMovementType() => MovementType.Controller;

    public ControllerLocomotion(LivingEntity owner)
        : base(owner) {
        InputManager.instance.SetControllerEnabled(true);
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

    protected override Vector3 GetLookingDirection() {
        return InputManager.getStick(InputManager.ControllerStick.Right);
    }
}
