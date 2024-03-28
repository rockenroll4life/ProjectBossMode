using UnityEngine;
using RockUtils.GameEvents;

public class KeyboardLocomotion : Locomotion {
    private Vector2 horizontalInput;
    private Vector2 verticalInput;

    //  TODO: [Rock]: This should really be a dictionary instead of an array as we access it via the KeyBindingKeys and if they're values change
    //  it'll cause out of bounds errors
    readonly KeyBinding[] keyBindings = new KeyBinding[4];
    bool rotateTowardsMouse;

    public override MovementType GetMovementType() => MovementType.Keyboard;

    public override Vector3 GetMovementDirection() => IsMoving() ? new Vector3(horizontalInput.x + horizontalInput.y, 0, verticalInput.x + verticalInput.y) : Vector3.zero;

    public KeyboardLocomotion(LivingEntity owner)
        : base (owner) {

        EventManager.StartListening(GameEvents.Keybindings_Changed, KeyBindingsChanged);
        EventManager.StartListening(GameEvents.GameplaySettings_Changed, GameplaySettingsChanged);

        keyBindings[(int) KeyBindingKeys.MoveUp] = Settings.GetKeyBinding(KeyBindingKeys.MoveUp);
        keyBindings[(int) KeyBindingKeys.MoveDown] = Settings.GetKeyBinding(KeyBindingKeys.MoveDown);
        keyBindings[(int) KeyBindingKeys.MoveLeft] = Settings.GetKeyBinding(KeyBindingKeys.MoveLeft);
        keyBindings[(int) KeyBindingKeys.MoveRight] = Settings.GetKeyBinding(KeyBindingKeys.MoveRight);

        rotateTowardsMouse = Settings.GetGameplaySetting(GameplayOptions.RotateTowardsMouse) > 0;

        foreach (KeyBinding binding in keyBindings) {
            ButtonStartListening(binding.keyboard);
            ButtonStartListening(binding.controller);
        }
    }

    ~KeyboardLocomotion() {
        foreach (KeyBinding binding in keyBindings) {
            ButtonStopListening(binding.keyboard);
            ButtonStopListening(binding.controller);
        }
    }

    void ButtonStartListening(KeyCode key) {
        EventManager.StartListening(GameEvents.KeyboardButton_Pressed + (int) key, InputPressed);
        EventManager.StartListening(GameEvents.KeyboardButton_Released + (int) key, InputReleased);
    }

    void ButtonStopListening(KeyCode key) {
        EventManager.StopListening(GameEvents.KeyboardButton_Pressed + (int) key, InputPressed);
        EventManager.StopListening(GameEvents.KeyboardButton_Released + (int) key, InputReleased);
    }

    public override bool IsMoving() {
        return horizontalInput != Vector2.zero || verticalInput != Vector2.zero;
    }

    public override void StopMovement() {
        
    }

    void InputPressed(int param) {
        KeyCode key = (KeyCode) param;

        if (keyBindings[(int) KeyBindingKeys.MoveUp].IsBinding(key)) {
            verticalInput.x = 1;
        }
        if (keyBindings[(int) KeyBindingKeys.MoveDown].IsBinding(key)) {
            verticalInput.y = -1;
        }

        if (keyBindings[(int) KeyBindingKeys.MoveLeft].IsBinding(key)) {
            horizontalInput.x = -1;
        }
        if (keyBindings[(int) KeyBindingKeys.MoveRight].IsBinding(key)) {
            horizontalInput.y = 1;
        }
    }

    void InputReleased(int param) {
        KeyCode key = (KeyCode) param;

        if (keyBindings[(int) KeyBindingKeys.MoveUp].IsBinding(key)) {
            verticalInput.x = 0;
        }
        if (keyBindings[(int) KeyBindingKeys.MoveDown].IsBinding(key)) {
            verticalInput.y = 0;
        }

        if (keyBindings[(int) KeyBindingKeys.MoveLeft].IsBinding(key)) {
            horizontalInput.x = 0;
        }
        if (keyBindings[(int) KeyBindingKeys.MoveRight].IsBinding(key)) {
            horizontalInput.y = 0;
        }
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

    void KeyBindingsChanged(int param) {
        foreach (KeyBinding binding in keyBindings) {
            ButtonStopListening(binding.keyboard);
            ButtonStopListening(binding.controller);
        }

        keyBindings[(int) KeyBindingKeys.MoveUp] = Settings.GetKeyBinding(KeyBindingKeys.MoveUp);
        keyBindings[(int) KeyBindingKeys.MoveDown] = Settings.GetKeyBinding(KeyBindingKeys.MoveDown);
        keyBindings[(int) KeyBindingKeys.MoveLeft] = Settings.GetKeyBinding(KeyBindingKeys.MoveLeft);
        keyBindings[(int) KeyBindingKeys.MoveRight] = Settings.GetKeyBinding(KeyBindingKeys.MoveRight);

        foreach (KeyBinding binding in keyBindings) {
            ButtonStartListening(binding.keyboard);
            ButtonStartListening(binding.controller);
        }
    }

    void GameplaySettingsChanged(int param) {
        rotateTowardsMouse = Settings.GetGameplaySetting(GameplayOptions.RotateTowardsMouse) > 0;
    }
}
