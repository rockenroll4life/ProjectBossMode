using UnityEngine;
using RockUtils.GameEvents;

public class KeyboardLocomotion : Locomotion {
    private Vector2 horizontalInput;
    private Vector2 verticalInput;

    float speed;

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

        Debug.Log("Input Key: " + ((KeyCode) param).ToString());
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

    protected override void UpdateMovement() {
        base.UpdateMovement();

        Vector3 direction = new Vector3(horizontalInput.x + horizontalInput.y, 0, verticalInput.x + verticalInput.y).normalized;
        owner.transform.position += direction * speed * Time.deltaTime;
    }

    protected override Vector3 GetLookingDirection() {
        return Vector3.up;
    }
}
