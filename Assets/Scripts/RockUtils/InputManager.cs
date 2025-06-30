using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

//  TODO: [Rock]: Current this only supports 1 player, but it would be nice to upgrade this to support multiple players for future projects
namespace RockUtils {
    namespace GameEvents {
        public class InputManager : Singleton<InputManager> {
            public enum ControllerButtons {
                A = 0,
                B,
                X,
                Y,

                Left,
                Right,
                Up,
                Down,

                Left_Trigger,
                Right_Trigger,

                Left_Bumper,
                Right_Bumper,

                Reset,
                Select,

                __COUNT
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

            protected override void Awake() {
                base.Awake();

                Init();
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

            public static void SetControllerEnabled(bool enabled) {
                if (Instance != null) {
                    Instance.controllerEnabled = enabled;
                }
            }

            public static void AddInputListener(KeyCode key, Action<int> listener) {
                if (Instance == null) {
                    return;
                }

                if (Instance.inputDictionary.TryGetValue(key, out int numListeners)) {
                    numListeners++;
                    Instance.inputDictionary[key] = numListeners;
                } else {
                    numListeners++;
                    Instance.inputDictionary.Add(key, numListeners);
                }
            }

            public static void RemoveInputListener(KeyCode key, Action<int> listener) {
                if (Instance == null) {
                    return;
                }

                if (Instance.inputDictionary.TryGetValue(key, out int numListeners)) {
                    if (numListeners-- == 0) {
                        Instance.inputDictionary.Remove(key);
                    } else {
                        Instance.inputDictionary[key] = numListeners;
                    }
                }
            }

            public static int GameEventToKeyCode(int gameEvents) {
                //  KeyCode Pressed
                if (gameEvents >= (int) GameEvents.KeyboardButton_Pressed && gameEvents < (int) GameEvents.KeyboardButton_Released) {
                    return gameEvents;
                }
                //  KeyCode Released
                else if (gameEvents >= (int) GameEvents.KeyboardButton_Released && gameEvents < (int) GameEvents.KeyboardButton_Held) {
                    return gameEvents - (int) GameEvents.KeyboardButton_Released;
                }
                //  KeyCode Held
                else if (gameEvents >= (int) GameEvents.KeyboardButton_Held && gameEvents < (int) GameEvents.Mouse_Left_Press) {
                    return gameEvents - (int) GameEvents.KeyboardButton_Held;
                }

                return -1;
            }

            //  TODO: [Rock]: We need to create a system within the input manager for CHORDS (Multi-key inputs - E.G. Ctrl+W, Shift+LClick)

            private void Update() {
                //  Mouse Button Input
                for (int i = 0; i < (int) MouseButtons.Total; i++) {
                    if (Input.GetMouseButtonDown(i)) {
                        EventManager.TriggerEvent(GameEvents.Mouse_Left_Press + i, 0);
                    } else if (Input.GetMouseButtonUp(i)) {
                        EventManager.TriggerEvent(GameEvents.Mouse_Left_Release + i, 0);
                    } else if (Input.GetMouseButton(i)) {
                        EventManager.TriggerEvent(GameEvents.Mouse_Left_Held + i, 0);

                        //  TODO: [Rock]: We should look into packing the X and Y movement into a single game event and value
                        float mouseX = Input.GetAxisRaw("Mouse X");
                        if (mouseX != 0) {
                            int param = (int) (mouseX * 1000);
                            EventManager.TriggerEvent(GameEvents.Mouse_Left_Move_X + i, param);
                        }

                        float mouseY = Input.GetAxisRaw("Mouse Y");
                        if (mouseY != 0) {
                            int param = (int) (mouseY * 1000);
                            EventManager.TriggerEvent(GameEvents.Mouse_Left_Move_Y + i, param);
                        }
                    }
                }

                //  Mouse Wheel Input
                if (Input.mouseScrollDelta != Vector2.zero) {
                    int param = (int) (Input.mouseScrollDelta.y * 1000);
                    EventManager.TriggerEvent(GameEvents.Mouse_Scroll_Wheel, param);
                }

                //  Keyboard Input
                foreach (KeyValuePair<KeyCode, int> key in Instance.inputDictionary) {
                    if (Input.GetKeyDown(key.Key)) {
                        EventManager.TriggerEvent(GameEvents.KeyboardButton_Pressed + (int) key.Key, (int) key.Key);
                    } else if (Input.GetKeyUp(key.Key)) {
                        EventManager.TriggerEvent(GameEvents.KeyboardButton_Released + (int) key.Key, (int) key.Key);
                    } else if (Input.GetKey(key.Key)) {
                        EventManager.TriggerEvent(GameEvents.KeyboardButton_Held + (int) key.Key, (int) key.Key);
                    }
                }

                //  Controller support
                if (controllerEnabled) {
                    prevState = state;
                    //  HACK: Because we only support 1 controller while we're a Singleton we need to hack this. FIX ME!
                    state = GamePad.GetState(PlayerIndex.One);
                }

                //  Controller Stick Movement
                if (controllerEnabled) {
                    float leftXRot = getStick(ControllerStick.Left).x;
                    if (Math.Abs(leftXRot) > Mathf.Epsilon) {
                        int param = (int) (leftXRot * 1000);
                        EventManager.TriggerEvent(GameEvents.Controller_Stick_Left_X, param);
                    }

                    float rightXRot = getStick(ControllerStick.Right).x;
                    if (Math.Abs(rightXRot) > Mathf.Epsilon) {
                        int param = (int) (rightXRot * 1000);
                        EventManager.TriggerEvent(GameEvents.Controller_Stick_Right_X, param);
                    }

                    float leftYRot = getStick(ControllerStick.Left).z;
                    if (Math.Abs(leftYRot) > Mathf.Epsilon) {
                        int param = (int) (leftYRot * 1000);
                        EventManager.TriggerEvent(GameEvents.Controller_Stick_Left_Y, param);
                    }

                    float rightYRot = getStick(ControllerStick.Right).z;
                    if (Math.Abs(rightYRot) > Mathf.Epsilon) {
                        int param = (int) (rightYRot * 1000);
                        EventManager.TriggerEvent(GameEvents.Controller_Stick_Right_Y, param);
                    }

                    //  Controller Buttons
                    for (int i = 0; i < (int) ControllerButtons.__COUNT; i++) {
                        ControllerButtons button = (ControllerButtons) i;

                        if (WasPressed(button)) {
                            EventManager.TriggerEvent(GameEvents.Controller_Button_Press + i);
                        }
                        if (WasReleased(button)) {
                            EventManager.TriggerEvent(GameEvents.Controller_Button_Release + i);
                        }
                        if (IsHeld(button)) {
                            EventManager.TriggerEvent(GameEvents.Controller_Button_Held + i);
                        }
                    }
                }
            }

            //  Returns a Vector3 for ease of use
            public static Vector3 getStick(ControllerStick stick) {
                if (Instance == null && !Instance.controllerEnabled) {
                    return Vector3.zero;
                }

                if (stick == ControllerStick.Left) {
                    return new Vector3(Instance.state.ThumbSticks.Left.X, 0, Instance.state.ThumbSticks.Left.Y);
                } else {
                    return new Vector3(Instance.state.ThumbSticks.Right.X, 0, Instance.state.ThumbSticks.Right.Y);
                }
            }

            public static bool isUp(ControllerButtons button) {
                if (Instance == null || !Instance.controllerEnabled) {
                    return false;
                }

                if (Instance.state.IsConnected) {
                    switch (button) {
                        case ControllerButtons.A:
                            return isReleased(Instance.state.Buttons.A);
                        case ControllerButtons.B:
                            return isReleased(Instance.state.Buttons.B);
                        case ControllerButtons.X:
                            return isReleased(Instance.state.Buttons.X);
                        case ControllerButtons.Y:
                            return isReleased(Instance.state.Buttons.Y);

                        case ControllerButtons.Left:
                            return isReleased(Instance.state.DPad.Left);
                        case ControllerButtons.Right:
                            return isReleased(Instance.state.DPad.Right);
                        case ControllerButtons.Up:
                            return isReleased(Instance.state.DPad.Up);
                        case ControllerButtons.Down:
                            return isReleased(Instance.state.DPad.Down);

                        case ControllerButtons.Left_Trigger:
                            return isReleased(Instance.state.Triggers.Left);
                        case ControllerButtons.Right_Trigger:
                            return isReleased(Instance.state.Triggers.Right);

                        case ControllerButtons.Left_Bumper:
                            return isReleased(Instance.state.Buttons.LeftShoulder);
                        case ControllerButtons.Right_Bumper:
                            return isReleased(Instance.state.Buttons.RightShoulder);

                        case ControllerButtons.Reset:
                            return isReleased(Instance.state.Buttons.Back);
                        case ControllerButtons.Select:
                            return isReleased(Instance.state.Buttons.Start);
                    }
                }
                return false;
            }

            public static bool IsHeld(ControllerButtons button) {
                if (Instance == null || !Instance.controllerEnabled) {
                    return false;
                }

                if (Instance.state.IsConnected) {
                    switch (button) {
                        case ControllerButtons.A:
                            return isPressed(Instance.state.Buttons.A);
                        case ControllerButtons.B:
                            return isPressed(Instance.state.Buttons.B);
                        case ControllerButtons.X:
                            return isPressed(Instance.state.Buttons.X);
                        case ControllerButtons.Y:
                            return isPressed(Instance.state.Buttons.Y);

                        case ControllerButtons.Left:
                            return isPressed(Instance.state.DPad.Left);
                        case ControllerButtons.Right:
                            return isPressed(Instance.state.DPad.Right);
                        case ControllerButtons.Up:
                            return isPressed(Instance.state.DPad.Up);
                        case ControllerButtons.Down:
                            return isPressed(Instance.state.DPad.Down);

                        case ControllerButtons.Left_Trigger:
                            return isPressed(Instance.state.Triggers.Left);
                        case ControllerButtons.Right_Trigger:
                            return isPressed(Instance.state.Triggers.Right);

                        case ControllerButtons.Left_Bumper:
                            return isPressed(Instance.state.Buttons.LeftShoulder);
                        case ControllerButtons.Right_Bumper:
                            return isPressed(Instance.state.Buttons.RightShoulder);

                        case ControllerButtons.Reset:
                            return isPressed(Instance.state.Buttons.Back);
                        case ControllerButtons.Select:
                            return isPressed(Instance.state.Buttons.Start);
                    }
                }
                return false;
            }

            public static bool WasPressed(ControllerButtons button) {
                if (Instance == null || !Instance.controllerEnabled) {
                    return false;
                }

                if (Instance.state.IsConnected) {
                    switch (button) {
                        case ControllerButtons.A:
                            return isReleased(Instance.prevState.Buttons.A) && isPressed(Instance.state.Buttons.A);
                        case ControllerButtons.B:
                            return isReleased(Instance.prevState.Buttons.B) && isPressed(Instance.state.Buttons.B);
                        case ControllerButtons.X:
                            return isReleased(Instance.prevState.Buttons.X) && isPressed(Instance.state.Buttons.X);
                        case ControllerButtons.Y:
                            return isReleased(Instance.prevState.Buttons.Y) && isPressed(Instance.state.Buttons.Y);

                        case ControllerButtons.Left:
                            return isReleased(Instance.prevState.DPad.Left) && isPressed(Instance.state.DPad.Left);
                        case ControllerButtons.Right:
                            return isReleased(Instance.prevState.DPad.Right) && isPressed(Instance.state.DPad.Right);
                        case ControllerButtons.Up:
                            return isReleased(Instance.prevState.DPad.Up) && isPressed(Instance.state.DPad.Up);
                        case ControllerButtons.Down:
                            return isReleased(Instance.prevState.DPad.Down) && isPressed(Instance.state.DPad.Down);

                        case ControllerButtons.Left_Trigger:
                            return isReleased(Instance.prevState.Triggers.Left) && isPressed(Instance.state.Triggers.Left);
                        case ControllerButtons.Right_Trigger:
                            return isReleased(Instance.prevState.Triggers.Right) && isPressed(Instance.state.Triggers.Right);

                        case ControllerButtons.Left_Bumper:
                            return isReleased(Instance.prevState.Buttons.LeftShoulder) && isPressed(Instance.state.Buttons.LeftShoulder);
                        case ControllerButtons.Right_Bumper:
                            return isReleased(Instance.prevState.Buttons.RightShoulder) && isPressed(Instance.state.Buttons.RightShoulder);

                        case ControllerButtons.Reset:
                            return isReleased(Instance.prevState.Buttons.Back) && isPressed(Instance.state.Buttons.Back);
                        case ControllerButtons.Select:
                            return isReleased(Instance.prevState.Buttons.Start) && isPressed(Instance.state.Buttons.Start);
                    }
                }
                return false;
            }

            public static bool WasReleased(ControllerButtons button) {
                if (Instance == null || !Instance.controllerEnabled) {
                    return false;
                }

                if (Instance.state.IsConnected) {
                    switch (button) {
                        case ControllerButtons.A:
                            return isPressed(Instance.prevState.Buttons.A) && isReleased(Instance.state.Buttons.A);
                        case ControllerButtons.B:
                            return isPressed(Instance.prevState.Buttons.B) && isReleased(Instance.state.Buttons.B);
                        case ControllerButtons.X:
                            return isPressed(Instance.prevState.Buttons.X) && isReleased(Instance.state.Buttons.X);
                        case ControllerButtons.Y:
                            return isPressed(Instance.prevState.Buttons.Y) && isReleased(Instance.state.Buttons.Y);

                        case ControllerButtons.Left:
                            return isPressed(Instance.prevState.DPad.Left) && isReleased(Instance.state.DPad.Left);
                        case ControllerButtons.Right:
                            return isPressed(Instance.prevState.DPad.Right) && isReleased(Instance.state.DPad.Right);
                        case ControllerButtons.Up:
                            return isPressed(Instance.prevState.DPad.Up) && isReleased(Instance.state.DPad.Up);
                        case ControllerButtons.Down:
                            return isPressed(Instance.prevState.DPad.Down) && isReleased(Instance.state.DPad.Down);

                        case ControllerButtons.Left_Trigger:
                            return isPressed(Instance.prevState.Triggers.Left) && isReleased(Instance.state.Triggers.Left);
                        case ControllerButtons.Right_Trigger:
                            return isPressed(Instance.prevState.Triggers.Right) && isReleased(Instance.state.Triggers.Right);

                        case ControllerButtons.Left_Bumper:
                            return isPressed(Instance.prevState.Buttons.LeftShoulder) && isReleased(Instance.state.Buttons.LeftShoulder);
                        case ControllerButtons.Right_Bumper:
                            return isPressed(Instance.prevState.Buttons.RightShoulder) && isReleased(Instance.state.Buttons.RightShoulder);

                        case ControllerButtons.Reset:
                            return isPressed(Instance.prevState.Buttons.Back) && isReleased(Instance.state.Buttons.Back);
                        case ControllerButtons.Select:
                            return isPressed(Instance.prevState.Buttons.Start) && isReleased(Instance.state.Buttons.Start);
                    }
                }
                return false;
            }

            private static bool isPressed(ButtonState state) {
                if (Instance == null) {
                    return false;
                }

                return Instance.controllerEnabled && state == ButtonState.Pressed;
            }

            private static bool isPressed(float buttonValue) {
                if (Instance == null) {
                    return false;
                }

                return Instance.controllerEnabled && buttonValue > 0;
            }

            private static bool isReleased(ButtonState state) {
                if (Instance == null) {
                    return false;
                }

                return Instance.controllerEnabled && state == ButtonState.Released;
            }

            private static bool isReleased(float buttonValue) {
                if (Instance == null) {
                    return false;
                }

                return Instance.controllerEnabled && buttonValue <= Mathf.Epsilon;
            }
        }
    }
}