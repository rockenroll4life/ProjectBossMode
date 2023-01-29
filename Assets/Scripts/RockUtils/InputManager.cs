using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using RockUtils.GameEvents;

//  TODO: [Rock]: Current this only supports 1 player, but it would be nice to upgrade this to support multiple players for future projects
public class InputManager : MonoBehaviour {
    public enum ControllerButtons {
        A = 0,
        B,
        X,
        Y,

        Left,
        Right,
        Up,
        Down,

        Left_Bumper,
        Right_Bumper,

        Reset,
        Select,
    }

    public enum ControllerStick {
        Left = 0,
        Right,
    }

    public enum MouseButtons {
        Left = 0,
        Right,
        Middle,

        Total
    }

    public enum MouseRotation {
        X = 0,
        Y,
    }

    static InputManager inputManager;
    public static InputManager instance {
        get {
            if (!inputManager) {
                inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;

                if (!inputManager) {
                    Debug.LogError("Using this requires an InputManager on a GameObject within the scene");
                } else {
                    inputManager.Init();
                }
            }

            return inputManager;
        }
    }

    void Init() {
        if (inputDictionary == null) {
            inputDictionary = new Dictionary<KeyCode, int>();
        }
    }

    GamePadState prevState;
    GamePadState state;

    //  Input
    Dictionary<KeyCode, int> inputDictionary;

    //  Controller
    bool controllerEnabled = false;

    public void SetControllerEnabled(bool enabled) {
        controllerEnabled = enabled;
    }

    public static void AddInputListener(KeyCode key, Action<int> listener) {
        if (instance.inputDictionary.TryGetValue(key, out int numListeners)) {
            numListeners++;
            instance.inputDictionary[key] = numListeners;
        } else {
            numListeners++;
            instance.inputDictionary.Add(key, numListeners);
        }

        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);
    }

    public static void RemoveInputListener(KeyCode key, Action<int> listener) {
        if (instance.inputDictionary.TryGetValue(key, out int numListeners)) {
            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);

            if (numListeners-- == 0) {
                instance.inputDictionary.Remove(key);
            } else {
                instance.inputDictionary[key] = numListeners;
            }

            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);
        }
    }

    private void Update() {
        //  Mouse Button Input
        for (int i = 0; i < (int) MouseButtons.Total; i++) {
            if (Input.GetMouseButtonDown(i)) {
                EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Press + i, 0);
            } else if (Input.GetMouseButtonUp(i)) {
                EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Release + i, 0);
            } else if (Input.GetMouseButton(i)) {
                EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Held + i, 0);

                //  TODO: [Rock]: We should look into packing the X and Y movement into a single game event and value
                float mouseX = Input.GetAxisRaw("Mouse X");
                if (mouseX != 0) {
                    int param = (int) (mouseX * 1000);
                    EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Move_X + i, param);
                }

                float mouseY = Input.GetAxisRaw("Mouse Y");
                if (mouseY != 0) {
                    int param = (int) (mouseY * 1000);
                    EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Move_Y + i, param);
                }
            }
        }

        //  Mouse Wheel Input
        if (Input.mouseScrollDelta != Vector2.zero) {
            int param = (int) (Input.mouseScrollDelta.y * 1000);
            EventManager.TriggerEvent((int) GameEvents.Mouse_Scroll_Wheel, param);
        }

        //  Keyboard Input
        foreach (KeyValuePair<KeyCode, int> key in instance.inputDictionary) {
            if (Input.GetKeyDown(key.Key)) {
                //  TODO: [Rock]: Should we pass any value back?
                EventManager.TriggerEvent((int) GameEvents.KeyboardButton_Pressed + (int) key.Key, 0);
            }
        }

        //  Controller support
        if (controllerEnabled) {
            prevState = state;
            //  HACK: Because we only support 1 controller while we're a Singleton we need to hack this. FIX ME!
            state = GamePad.GetState(PlayerIndex.One);
        }

        //  Mouse Rotation
        //  TODO
        //  NOTE: Mouse Rotation and controller rotation do the same thing, just in different ways...FIGURE IT OUT! XD

        //  Controller Rotation
        if (controllerEnabled) {
            float leftXRot = getStick(ControllerStick.Left).x;
            if (Math.Abs(leftXRot) > Mathf.Epsilon) {
                int param = (int) (leftXRot * 1000);
                EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Move_X, param);
            }

            float rightXRot = getStick(ControllerStick.Right).x;
            if (Math.Abs(rightXRot) > Mathf.Epsilon) {
                int param = (int) (rightXRot * 1000);
                EventManager.TriggerEvent((int) GameEvents.Mouse_Right_Move_X, param);
            }

            float leftYRot = getStick(ControllerStick.Left).z;
            if (Math.Abs(leftYRot) > Mathf.Epsilon) {
                int param = (int) (leftYRot * 1000);
                EventManager.TriggerEvent((int) GameEvents.Mouse_Left_Move_Y, param);
            }

            float rightYRot = getStick(ControllerStick.Right).z;
            if (Math.Abs(rightYRot) > Mathf.Epsilon) {
                int param = (int) (rightYRot * 1000);
                EventManager.TriggerEvent((int) GameEvents.Mouse_Right_Move_Y, param);
            }
        }
    }

    //  Returns a Vector3 for ease of use
    public static Vector3 getStick(ControllerStick stick) {
        if (!instance.controllerEnabled) {
            return Vector3.zero;
        }

        if (stick == ControllerStick.Left) {
            return new Vector3(instance.state.ThumbSticks.Left.X, 0, instance.state.ThumbSticks.Left.Y);
        } else {
            return new Vector3(instance.state.ThumbSticks.Right.X, 0, instance.state.ThumbSticks.Right.Y);
        }
    }

    public static bool isUp(ControllerButtons button) {
        if (!instance.controllerEnabled) {
            return false;
        }

        if (instance.state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isReleased(instance.state.Buttons.A);
                case ControllerButtons.B:
                    return isReleased(instance.state.Buttons.B);
                case ControllerButtons.X:
                    return isReleased(instance.state.Buttons.X);
                case ControllerButtons.Y:
                    return isReleased(instance.state.Buttons.Y);

                case ControllerButtons.Left:
                    return isReleased(instance.state.DPad.Left);
                case ControllerButtons.Right:
                    return isReleased(instance.state.DPad.Right);
                case ControllerButtons.Up:
                    return isReleased(instance.state.DPad.Up);
                case ControllerButtons.Down:
                    return isReleased(instance.state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isReleased(instance.state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isReleased(instance.state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isReleased(instance.state.Buttons.Back);
                case ControllerButtons.Select:
                    return isReleased(instance.state.Buttons.Start);
            }
        }
        return false;
    }

    public static bool isDown(ControllerButtons button) {
        if (!instance.controllerEnabled) {
            return false;
        }

        if (instance.state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isPressed(instance.state.Buttons.A);
                case ControllerButtons.B:
                    return isPressed(instance.state.Buttons.B);
                case ControllerButtons.X:
                    return isPressed(instance.state.Buttons.X);
                case ControllerButtons.Y:
                    return isPressed(instance.state.Buttons.Y);

                case ControllerButtons.Left:
                    return isPressed(instance.state.DPad.Left);
                case ControllerButtons.Right:
                    return isPressed(instance.state.DPad.Right);
                case ControllerButtons.Up:
                    return isPressed(instance.state.DPad.Up);
                case ControllerButtons.Down:
                    return isPressed(instance.state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isPressed(instance.state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isPressed(instance.state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isPressed(instance.state.Buttons.Back);
                case ControllerButtons.Select:
                    return isPressed(instance.state.Buttons.Start);
            }
        }
        return false;
    }

    public static bool wasPressed(ControllerButtons button) {
        if (!instance.controllerEnabled) {
            return false;
        }

        if (instance.state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isReleased(instance.prevState.Buttons.A) && isPressed(instance.state.Buttons.A);
                case ControllerButtons.B:
                    return isReleased(instance.prevState.Buttons.B) && isPressed(instance.state.Buttons.B);
                case ControllerButtons.X:
                    return isReleased(instance.prevState.Buttons.X) && isPressed(instance.state.Buttons.X);
                case ControllerButtons.Y:
                    return isReleased(instance.prevState.Buttons.Y) && isPressed(instance.state.Buttons.Y);

                case ControllerButtons.Left:
                    return isReleased(instance.prevState.DPad.Left) && isPressed(instance.state.DPad.Left);
                case ControllerButtons.Right:
                    return isReleased(instance.prevState.DPad.Right) && isPressed(instance.state.DPad.Right);
                case ControllerButtons.Up:
                    return isReleased(instance.prevState.DPad.Up) && isPressed(instance.state.DPad.Up);
                case ControllerButtons.Down:
                    return isReleased(instance.prevState.DPad.Down) && isPressed(instance.state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isReleased(instance.prevState.Buttons.LeftShoulder) && isPressed(instance.state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isReleased(instance.prevState.Buttons.RightShoulder) && isPressed(instance.state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isReleased(instance.prevState.Buttons.Back) && isPressed(instance.state.Buttons.Back);
                case ControllerButtons.Select:
                    return isReleased(instance.prevState.Buttons.Start) && isPressed(instance.state.Buttons.Start);
            }
        }
        return false;
    }

    private static bool isPressed(ButtonState state) {
        return instance.controllerEnabled && state == ButtonState.Pressed;
    }

    private static bool isReleased(ButtonState state) {
        return instance.controllerEnabled && state == ButtonState.Released;
    }
}