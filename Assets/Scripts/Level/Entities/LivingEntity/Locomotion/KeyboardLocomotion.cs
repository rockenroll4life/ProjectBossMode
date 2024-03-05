using UnityEngine;
using RockUtils.GameEvents;

public class KeyboardLocomotion : Locomotion {
    private Vector2 horizontalInput;
    private Vector2 verticalInput;

    readonly KeyCode[] keyBindings = new KeyCode[4];
    bool rotateTowardsMouse;
    float speed;

    public override MovementType GetMovementType() => MovementType.Keyboard;

    public KeyboardLocomotion(LivingEntity owner)
        : base (owner) {

        EventManager.StartListening((int) GameEvents.Keybindings_Changed, KeyBindingsChanged);
        EventManager.StartListening((int) GameEvents.GameplaySettings_Changed, GameplaySettingsChanged);

        speed = owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue();

        keyBindings[(int) KeyBindingKeys.MoveUp] = Settings.GetKeyBinding(KeyBindingKeys.MoveUp);
        keyBindings[(int) KeyBindingKeys.MoveDown] = Settings.GetKeyBinding(KeyBindingKeys.MoveDown);
        keyBindings[(int) KeyBindingKeys.MoveLeft] = Settings.GetKeyBinding(KeyBindingKeys.MoveLeft);
        keyBindings[(int) KeyBindingKeys.MoveRight] = Settings.GetKeyBinding(KeyBindingKeys.MoveRight);

        rotateTowardsMouse = Settings.GetGameplaySetting(GameplayOptions.RotateTowardsMouse) > 0;

        foreach (KeyCode key in keyBindings) {
            ButtonStartListening(key);
        }
    }

    ~KeyboardLocomotion() {
        foreach (KeyCode key in keyBindings) {
            ButtonStopListening(key);
        }
    }

    void ButtonStartListening(KeyCode key) {
        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) key, InputPressed);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) key, InputReleased);
    }

    void ButtonStopListening(KeyCode key) {
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, InputPressed);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) key, InputReleased);
    }

    public override bool IsMoving() {
        return horizontalInput != Vector2.zero || verticalInput != Vector2.zero;
    }

    public override void StopMovement() {
        
    }

    void InputPressed(int param) {
        KeyCode key = (KeyCode) param;

        if (key == keyBindings[(int) KeyBindingKeys.MoveUp]) {
            verticalInput.x = 1;
        }
        if (key == keyBindings[(int) KeyBindingKeys.MoveDown]) {
            verticalInput.y = -1;
        }

        if (key == keyBindings[(int) KeyBindingKeys.MoveLeft]) {
            horizontalInput.x = -1;
        }
        if (key == keyBindings[(int) KeyBindingKeys.MoveRight]) {
            horizontalInput.y = 1;
        }
    }

    void InputReleased(int param) {
        KeyCode key = (KeyCode) param;

        if (key == keyBindings[(int) KeyBindingKeys.MoveUp]) {
            verticalInput.x = 0;
        }
        if (key == keyBindings[(int) KeyBindingKeys.MoveDown]) {
            verticalInput.y = 0;
        }

        if (key == keyBindings[(int) KeyBindingKeys.MoveLeft]) {
            horizontalInput.x = 0;
        }
        if (key == keyBindings[(int) KeyBindingKeys.MoveRight]) {
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

    protected override void SpeedChanged(int param) {
        speed = param / 1000f;
    }

    void KeyBindingsChanged(int param) {
        foreach (KeyCode key in keyBindings) {
            ButtonStopListening(key);
        }

        keyBindings[(int) KeyBindingKeys.MoveUp] = Settings.GetKeyBinding(KeyBindingKeys.MoveUp);
        keyBindings[(int) KeyBindingKeys.MoveDown] = Settings.GetKeyBinding(KeyBindingKeys.MoveDown);
        keyBindings[(int) KeyBindingKeys.MoveLeft] = Settings.GetKeyBinding(KeyBindingKeys.MoveLeft);
        keyBindings[(int) KeyBindingKeys.MoveRight] = Settings.GetKeyBinding(KeyBindingKeys.MoveRight);

        foreach (KeyCode key in keyBindings) {
            ButtonStartListening(key);
        }
    }

    void GameplaySettingsChanged(int param) {
        rotateTowardsMouse = Settings.GetGameplaySetting(GameplayOptions.RotateTowardsMouse) > 0;
    }
}
