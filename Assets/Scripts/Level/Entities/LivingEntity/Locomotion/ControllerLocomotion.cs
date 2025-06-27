using RockUtils.GameEvents;
using UnityEngine;

public class ControllerLocomotion : Locomotion {
    static readonly int UPDATE_THRESHOLD = 3;

    private Vector2 leftStick;
    private Vector2 rightStick;
    int updatesSinceLastInput = 0;

    public override MovementType GetMovementType() => MovementType.Controller;

    protected override Vector3 GetLookingDirection() => new Vector3(rightStick.x, 0, rightStick.y).normalized;
    public override Vector3 GetMovementDirection() => new Vector3(leftStick.x, 0, leftStick.y).normalized;
    
    public override bool IsMoving() => GetMovementDirection() != Vector3.zero;

    public ControllerLocomotion(LivingEntity owner)
        : base(owner) {
        InputManager.SetControllerEnabled(true);

        EventManager.StartListening(GameEvents.Controller_Stick_Left_X, LeftStickMovementX);
        EventManager.StartListening(GameEvents.Controller_Stick_Left_Y, LeftStickMovementY);
        EventManager.StartListening(GameEvents.Controller_Stick_Right_X, RightStickMovementX);
        EventManager.StartListening(GameEvents.Controller_Stick_Right_Y, RightStickMovementY);
    }

    ~ControllerLocomotion() {
        InputManager.SetControllerEnabled(false);
    }

    void LeftStickMovementX(int param) {
        float value = param * 1000f;
        leftStick.x = value;
        updatesSinceLastInput = 0;
    }
    
    void LeftStickMovementY(int param) {
        float value = param * 1000f;
        leftStick.y = value;
        updatesSinceLastInput = 0;
    }
    
    void RightStickMovementX(int param) {
        float value = param * 1000f;
        rightStick.x = value;
    }
    
    void RightStickMovementY(int param) {
        float value = param * 1000f;
        rightStick.y = value;
    }

    protected override void UpdateMovement() {
        base.UpdateMovement();

        if (updatesSinceLastInput++ >= UPDATE_THRESHOLD) {
            leftStick = Vector3.zero;
        }
    }

    public override void StopMovement() {
        
    }
}
