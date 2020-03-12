using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

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

    static InputManager inputManager;
    public static InputManager instance {
        get {
            if (!inputManager) {
                inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;

                if (!inputManager) {
                    Debug.LogError("Using this requires an EventManager on a GameObject within the scene");
                } else {
                    inputManager.Init();
                }
            }

            return inputManager;
        }
    }

    void Init() {
        if (keyboardDictionary == null) {
            keyboardDictionary = new Dictionary<KeyCode, int>();
        }
    }

    GamePadState prevState;
    GamePadState state;

    //  Keyboard
    Dictionary<KeyCode, int> keyboardDictionary;

    //  Controller
    bool controllerEnabled = false;

    public void SetControllerEnable(bool enabled) {
        controllerEnabled = enabled;
    }

    public static void AddKeyboardListener(KeyCode key, Action listener) {
        if (instance.keyboardDictionary.TryGetValue(key, out int numListeners)) {
            numListeners++;
            instance.keyboardDictionary[key] = numListeners;
        } else {
            numListeners++;
            instance.keyboardDictionary.Add(key, numListeners);
        }

        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);
    }

    public static void RemoveKeyboardListener(KeyCode key, Action listener) {
        if (instance.keyboardDictionary.TryGetValue(key, out int numListeners)) {
            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);

            if (numListeners-- == 0) {
                instance.keyboardDictionary.Remove(key);
            } else {
                instance.keyboardDictionary[key] = numListeners;
            }

            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);
        }
    }

    private void Update() {
        //  Mouse Input
        for (int i = 0; i < (int) MouseButtons.Total; i++) {
            if (Input.GetMouseButtonUp(i)) {
                EventManager.TriggerEvent((int) GameEvents.Mouse_LeftClick + i);
            }
        }

        //  Keyboard Input
        foreach (KeyValuePair<KeyCode, int> key in instance.keyboardDictionary) {
            if (Input.GetKeyUp(key.Key)) {
                EventManager.TriggerEvent((int) GameEvents.KeyboardButton_Pressed + (int) key.Key);
            }
        }

        //  Controller support
        if (controllerEnabled) {
            prevState = state;
            //  HACK: Because we only support 1 controller while we're a Singleton we need to hack this. FIX ME!
            state = GamePad.GetState(PlayerIndex.One);
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