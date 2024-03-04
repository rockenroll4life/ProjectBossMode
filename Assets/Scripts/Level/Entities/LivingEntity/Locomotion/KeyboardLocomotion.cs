using UnityEngine;
using RockUtils.GameEvents;

public class KeyboardLocomotion : Locomotion {
    private Vector2 horizontalInput;
    private Vector2 verticalInput;

    bool rotateTowardsMouse = true;
    float speed;

    public override MovementType GetMovementType() => MovementType.Keyboard;

    public KeyboardLocomotion(LivingEntity owner)
        : base (owner) {

        speed = owner.GetAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED).GetValue();

        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.W, InputPressed);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.S, InputPressed);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.A, InputPressed);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.D, InputPressed);

        EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.W, InputReleased);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.S, InputReleased);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.A, InputReleased);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.D, InputReleased);
    }

    ~KeyboardLocomotion() {
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.W, InputPressed);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.S, InputPressed);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.A, InputPressed);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.D, InputPressed);
        
        EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.W, InputReleased);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.S, InputReleased);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.A, InputReleased);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) KeyCode.D, InputReleased);
    }

    public override bool IsMoving() {
        return horizontalInput != Vector2.zero || verticalInput != Vector2.zero;
    }

    public override void StopMovement() {
        
    }

    void InputPressed(int param) {
        KeyCode key = (KeyCode) param;

        if (key == KeyCode.W) {
            verticalInput.x = 1;
        } if (key == KeyCode.S) {
            verticalInput.y = -1;
        }

        if (key == KeyCode.A) {
            horizontalInput.x = -1;
        }
        if (key == KeyCode.D) {
            horizontalInput.y = 1;
        }
    }

    void InputReleased(int param) {
        KeyCode key = (KeyCode) param;

        if (key == KeyCode.W) {
            verticalInput.x = 0;
        }
        if (key == KeyCode.S) {
            verticalInput.y = 0;
        }

        if (key == KeyCode.A) {
            horizontalInput.x = 0;
        }
        if (key == KeyCode.D) {
            horizontalInput.y = 0;
        }
    }

    protected override void UpdateRotation() {
        base.UpdateRotation();

        Vector3 lookDir = GetLookingDirection();
        if (lookDir != Vector3.zero) {
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(GetLookingDirection()), ROTATION_SPEED * Time.deltaTime);
        }
    }

    protected override void UpdateMovement() {
        base.UpdateMovement();

        Vector3 direction = new Vector3(horizontalInput.x + horizontalInput.y, 0, verticalInput.x + verticalInput.y).normalized;
        owner.transform.position += direction * speed * Time.deltaTime;
    }

    protected override Vector3 GetLookingDirection() {
        if (rotateTowardsMouse) {
            Vector3 mousePos = Input.mousePosition;
            Plane plane = new Plane(Camera.main.transform.forward, owner.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Vector3 direction = Vector3.forward;

            float distance;
            if (plane.Raycast(ray, out distance)) {
                Vector3 worldPos = ray.GetPoint(distance);

                direction = worldPos - owner.transform.position;
                direction.y = 0;
            }

            return direction;
        } else {
            if (IsMoving()) {
                return new Vector3(horizontalInput.x + horizontalInput.y, 0, verticalInput.x + verticalInput.y);
            } else {
                return owner.transform.forward;
            }
        }

    }
}
