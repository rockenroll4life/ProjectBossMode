using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

//  TODO: [Rock]: Turn this into a Singleton Monobehavior so it doesn't need to be updated in the player (We won't always have one)
public class InputManager {
    public enum ControllerButtons {
        A,
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
        Left,
        Right,
    }

    public enum MouseButtons {
        Left = 0,
        Right,
        Middle,

        Total
    }

    readonly PlayerIndex indexID;
    GamePadState prevState;
    GamePadState state;

    //  Keyboard
    Dictionary<KeyCode, int> keyboardDictionary = new Dictionary<KeyCode, int>();

    //  Controller
    bool controllerEnabled = false;

    public InputManager(PlayerIndex indexID) {
        this.indexID = indexID;
    }

    public void SetControllerEnable(bool enabled) {
        controllerEnabled = enabled;
    }

    public void AddKeyboardListener(KeyCode key, Action listener) {
        if (keyboardDictionary.TryGetValue(key, out int numListeners)) {
            numListeners++;
            keyboardDictionary[key] = numListeners;
        } else {
            numListeners++;
            keyboardDictionary.Add(key, numListeners);
        }

        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);
    }

    public void RemoveKeyboardListener(KeyCode key, Action listener) {
        if (keyboardDictionary.TryGetValue(key, out int numListeners)) {
            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) key, listener);

            if (numListeners-- == 0) {
                keyboardDictionary.Remove(key);
            } else {
                keyboardDictionary[key] = numListeners;
            }
        }
    }

    public void update() {
        //  Mouse Input
        for (int i = 0; i < (int) MouseButtons.Total; i++) {
            if (Input.GetMouseButtonUp(i)) {
                EventManager.TriggerEvent((int) GameEvents.Mouse_LeftClick + i);
            }
        }

        //  Keyboard Input
        foreach (KeyValuePair<KeyCode, int> key in keyboardDictionary) {
            if (Input.GetKeyUp(key.Key)) {
                EventManager.TriggerEvent((int) GameEvents.KeyboardButton_Pressed + (int) key.Key);
            }
        }

        //  Controller support
        if (controllerEnabled) {
            prevState = state;
            state = GamePad.GetState(indexID);
        }
    }

    //  Returns a Vector3 for ease of use
    public Vector3 getStick(ControllerStick stick) {
        if (!controllerEnabled) {
            return Vector3.zero;
        }

        if (stick == ControllerStick.Left) {
            return new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        } else {
            return new Vector3(state.ThumbSticks.Right.X, 0, state.ThumbSticks.Right.Y);
        }
    }

    public bool isUp(ControllerButtons button) {
        if (!controllerEnabled) {
            return false;
        }

        if (state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isReleased(state.Buttons.A);
                case ControllerButtons.B:
                    return isReleased(state.Buttons.B);
                case ControllerButtons.X:
                    return isReleased(state.Buttons.X);
                case ControllerButtons.Y:
                    return isReleased(state.Buttons.Y);

                case ControllerButtons.Left:
                    return isReleased(state.DPad.Left);
                case ControllerButtons.Right:
                    return isReleased(state.DPad.Right);
                case ControllerButtons.Up:
                    return isReleased(state.DPad.Up);
                case ControllerButtons.Down:
                    return isReleased(state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isReleased(state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isReleased(state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isReleased(state.Buttons.Back);
                case ControllerButtons.Select:
                    return isReleased(state.Buttons.Start);
            }
        }
        return false;
    }

    public bool isDown(ControllerButtons button) {
        if (!controllerEnabled) {
            return false;
        }

        if (state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isPressed(state.Buttons.A);
                case ControllerButtons.B:
                    return isPressed(state.Buttons.B);
                case ControllerButtons.X:
                    return isPressed(state.Buttons.X);
                case ControllerButtons.Y:
                    return isPressed(state.Buttons.Y);

                case ControllerButtons.Left:
                    return isPressed(state.DPad.Left);
                case ControllerButtons.Right:
                    return isPressed(state.DPad.Right);
                case ControllerButtons.Up:
                    return isPressed(state.DPad.Up);
                case ControllerButtons.Down:
                    return isPressed(state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isPressed(state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isPressed(state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isPressed(state.Buttons.Back);
                case ControllerButtons.Select:
                    return isPressed(state.Buttons.Start);
            }
        }
        return false;
    }

    public bool wasPressed(ControllerButtons button) {
        if (!controllerEnabled) {
            return false;
        }

        if (state.IsConnected) {
            switch (button) {
                case ControllerButtons.A:
                    return isReleased(prevState.Buttons.A) && isPressed(state.Buttons.A);
                case ControllerButtons.B:
                    return isReleased(prevState.Buttons.B) && isPressed(state.Buttons.B);
                case ControllerButtons.X:
                    return isReleased(prevState.Buttons.X) && isPressed(state.Buttons.X);
                case ControllerButtons.Y:
                    return isReleased(prevState.Buttons.Y) && isPressed(state.Buttons.Y);

                case ControllerButtons.Left:
                    return isReleased(prevState.DPad.Left) && isPressed(state.DPad.Left);
                case ControllerButtons.Right:
                    return isReleased(prevState.DPad.Right) && isPressed(state.DPad.Right);
                case ControllerButtons.Up:
                    return isReleased(prevState.DPad.Up) && isPressed(state.DPad.Up);
                case ControllerButtons.Down:
                    return isReleased(prevState.DPad.Down) && isPressed(state.DPad.Down);

                case ControllerButtons.Left_Bumper:
                    return isReleased(prevState.Buttons.LeftShoulder) && isPressed(state.Buttons.LeftShoulder);
                case ControllerButtons.Right_Bumper:
                    return isReleased(prevState.Buttons.RightShoulder) && isPressed(state.Buttons.RightShoulder);

                case ControllerButtons.Reset:
                    return isReleased(prevState.Buttons.Back) && isPressed(state.Buttons.Back);
                case ControllerButtons.Select:
                    return isReleased(prevState.Buttons.Start) && isPressed(state.Buttons.Start);
            }
        }
        return false;
    }

    private bool isPressed(ButtonState state) {
        return controllerEnabled &&  state == ButtonState.Pressed;
    }

    private bool isReleased(ButtonState state) {
        return controllerEnabled && state == ButtonState.Released;
    }
}