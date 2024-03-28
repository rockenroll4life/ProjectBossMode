using RockUtils.GameEvents;
using UnityEngine;

public class ControllerLocomotion : Locomotion {
    public override MovementType GetMovementType() => MovementType.Controller;

    protected override Vector3 GetLookingDirection() => InputManager.getStick(InputManager.ControllerStick.Right);
    public override Vector3 GetMovementDirection() => InputManager.getStick(InputManager.ControllerStick.Left);
    
    public override bool IsMoving() => GetMovementDirection() != Vector3.zero;

    public ControllerLocomotion(LivingEntity owner)
        : base(owner) {
        InputManager.instance.SetControllerEnabled(true);
    }

    ~ControllerLocomotion() {
        InputManager.instance.SetControllerEnabled(false);
    }

    public override void StopMovement() {
        
    }
}
