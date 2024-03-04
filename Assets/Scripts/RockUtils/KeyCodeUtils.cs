using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace KeyCodeUtils {
        public class KeyCodeUtils {
            public static string ToCaption(KeyCode key) {
                switch (key) {
                    case KeyCode.Exclaim:           return "Exclamation Mark";
                    case KeyCode.DoubleQuote:       return "Double Quote";
                    case KeyCode.Dollar:            return "Dollar Sign";
                    case KeyCode.LeftParen:         return "Left Parenthesis";
                    case KeyCode.RightParen:        return "Right Parenthesis";

                    case KeyCode.Alpha0:
                    case KeyCode.Alpha1:
                    case KeyCode.Alpha2:
                    case KeyCode.Alpha3:
                    case KeyCode.Alpha4:
                    case KeyCode.Alpha5:
                    case KeyCode.Alpha6:
                    case KeyCode.Alpha7:
                    case KeyCode.Alpha8:
                    case KeyCode.Alpha9:
                        return $"{key - KeyCode.Alpha0}";

                    case KeyCode.Less:              return "Less than";
                    case KeyCode.Greater:           return "Greater than";
                    case KeyCode.Question:          return "Question Mark";
                    case KeyCode.LeftBracket:       return "Left Square Bracket";
                    case KeyCode.RightBracket:      return "Right Square Bracket";
                    case KeyCode.BackQuote:         return "Back Quote";
                    case KeyCode.LeftCurlyBracket:  return "Left Curly Bracket";
                    case KeyCode.RightCurlyBracket: return "Right Curly Bracket";
                    case KeyCode.Keypad0:           return "Keypad 0";
                    case KeyCode.Keypad1:           return "Keypad 1";
                    case KeyCode.Keypad2:           return "Keypad 2";
                    case KeyCode.Keypad3:           return "Keypad 3";
                    case KeyCode.Keypad4:           return "Keypad 4";
                    case KeyCode.Keypad5:           return "Keypad 5";
                    case KeyCode.Keypad6:           return "Keypad 6";
                    case KeyCode.Keypad7:           return "Keypad 7";
                    case KeyCode.Keypad8:           return "Keypad 8";
                    case KeyCode.Keypad9:           return "Keypad 9";
                    case KeyCode.KeypadPeriod:      return "Keypad Period";
                    case KeyCode.KeypadDivide:      return "Keypad Divide";
                    case KeyCode.KeypadMultiply:    return "Keypad Multiply";
                    case KeyCode.KeypadMinus:       return "Keypad Minus";
                    case KeyCode.KeypadPlus:        return "Keypad Plus";
                    case KeyCode.KeypadEnter:       return "Keypad Enter";
                    case KeyCode.KeypadEquals:      return "Keypad Equals";
                    case KeyCode.UpArrow:           return "Up Arrow";
                    case KeyCode.DownArrow:         return "Down Arrow";
                    case KeyCode.RightArrow:        return "Right Arrow";
                    case KeyCode.LeftArrow:         return "Left Arrow";
                    case KeyCode.PageUp:            return "Page Up";
                    case KeyCode.PageDown:          return "Page Down";
                    case KeyCode.ScrollLock:        return "Scroll Lock";
                    case KeyCode.RightShift:        return "Right Shift";
                    case KeyCode.LeftShift:         return "Left Shift";
                    case KeyCode.RightControl:      return "Right Control";
                    case KeyCode.LeftControl:       return "Left Control";
                    case KeyCode.RightAlt:          return "Right Alt";
                    case KeyCode.LeftAlt:           return "Left Alt";
                    
                   // Mouse Inputs
                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                    case KeyCode.Mouse3:
                    case KeyCode.Mouse4:
                    case KeyCode.Mouse5:
                    case KeyCode.Mouse6:
                        return $"Mouse {key - KeyCode.Mouse0}";

                    //  Joystick buttons
                    case KeyCode.JoystickButton0:
                    case KeyCode.JoystickButton1:
                    case KeyCode.JoystickButton2:
                    case KeyCode.JoystickButton3:
                    case KeyCode.JoystickButton4:
                    case KeyCode.JoystickButton5:
                    case KeyCode.JoystickButton6:
                    case KeyCode.JoystickButton7:
                    case KeyCode.JoystickButton8:
                    case KeyCode.JoystickButton9:
                    case KeyCode.JoystickButton10:
                    case KeyCode.JoystickButton11:
                    case KeyCode.JoystickButton12:
                    case KeyCode.JoystickButton13:
                    case KeyCode.JoystickButton14:
                    case KeyCode.JoystickButton15:
                    case KeyCode.JoystickButton16:
                    case KeyCode.JoystickButton17:
                    case KeyCode.JoystickButton18:
                    case KeyCode.JoystickButton19:
                        return $"Joystick Button {key - KeyCode.JoystickButton0}";

                    //  Joystick 1 buttons
                    case KeyCode.Joystick1Button0:
                    case KeyCode.Joystick1Button1:
                    case KeyCode.Joystick1Button2:
                    case KeyCode.Joystick1Button3:
                    case KeyCode.Joystick1Button4:
                    case KeyCode.Joystick1Button5:
                    case KeyCode.Joystick1Button6:
                    case KeyCode.Joystick1Button7:
                    case KeyCode.Joystick1Button8:
                    case KeyCode.Joystick1Button9:
                    case KeyCode.Joystick1Button10:
                    case KeyCode.Joystick1Button11:
                    case KeyCode.Joystick1Button12:
                    case KeyCode.Joystick1Button13:
                    case KeyCode.Joystick1Button14:
                    case KeyCode.Joystick1Button15:
                    case KeyCode.Joystick1Button16:
                    case KeyCode.Joystick1Button17:
                    case KeyCode.Joystick1Button18:
                    case KeyCode.Joystick1Button19:
                        return $"Joystick 1 Button {key - KeyCode.Joystick1Button0}";

                    //  Joystick 2 buttons
                    case KeyCode.Joystick2Button0:
                    case KeyCode.Joystick2Button1:
                    case KeyCode.Joystick2Button2:
                    case KeyCode.Joystick2Button3:
                    case KeyCode.Joystick2Button4:
                    case KeyCode.Joystick2Button5:
                    case KeyCode.Joystick2Button6:
                    case KeyCode.Joystick2Button7:
                    case KeyCode.Joystick2Button8:
                    case KeyCode.Joystick2Button9:
                    case KeyCode.Joystick2Button10:
                    case KeyCode.Joystick2Button11:
                    case KeyCode.Joystick2Button12:
                    case KeyCode.Joystick2Button13:
                    case KeyCode.Joystick2Button14:
                    case KeyCode.Joystick2Button15:
                    case KeyCode.Joystick2Button16:
                    case KeyCode.Joystick2Button17:
                    case KeyCode.Joystick2Button18:
                    case KeyCode.Joystick2Button19:
                        return $"Joystick 2 Button {key - KeyCode.Joystick2Button0}";

                    //  Joystick 3 buttons
                    case KeyCode.Joystick3Button0:
                    case KeyCode.Joystick3Button1:
                    case KeyCode.Joystick3Button2:
                    case KeyCode.Joystick3Button3:
                    case KeyCode.Joystick3Button4:
                    case KeyCode.Joystick3Button5:
                    case KeyCode.Joystick3Button6:
                    case KeyCode.Joystick3Button7:
                    case KeyCode.Joystick3Button8:
                    case KeyCode.Joystick3Button9:
                    case KeyCode.Joystick3Button10:
                    case KeyCode.Joystick3Button11:
                    case KeyCode.Joystick3Button12:
                    case KeyCode.Joystick3Button13:
                    case KeyCode.Joystick3Button14:
                    case KeyCode.Joystick3Button15:
                    case KeyCode.Joystick3Button16:
                    case KeyCode.Joystick3Button17:
                    case KeyCode.Joystick3Button18:
                    case KeyCode.Joystick3Button19:
                        return $"Joystick 3 Button {key - KeyCode.Joystick3Button0}";

                    //  Joystick 4 buttons
                    case KeyCode.Joystick4Button0:
                    case KeyCode.Joystick4Button1:
                    case KeyCode.Joystick4Button2:
                    case KeyCode.Joystick4Button3:
                    case KeyCode.Joystick4Button4:
                    case KeyCode.Joystick4Button5:
                    case KeyCode.Joystick4Button6:
                    case KeyCode.Joystick4Button7:
                    case KeyCode.Joystick4Button8:
                    case KeyCode.Joystick4Button9:
                    case KeyCode.Joystick4Button10:
                    case KeyCode.Joystick4Button11:
                    case KeyCode.Joystick4Button12:
                    case KeyCode.Joystick4Button13:
                    case KeyCode.Joystick4Button14:
                    case KeyCode.Joystick4Button15:
                    case KeyCode.Joystick4Button16:
                    case KeyCode.Joystick4Button17:
                    case KeyCode.Joystick4Button18:
                    case KeyCode.Joystick4Button19:
                        return $"Joystick 4 Button {key - KeyCode.Joystick4Button0}";

                    //  Joystick 5 buttons
                    case KeyCode.Joystick5Button0:
                    case KeyCode.Joystick5Button1:
                    case KeyCode.Joystick5Button2:
                    case KeyCode.Joystick5Button3:
                    case KeyCode.Joystick5Button4:
                    case KeyCode.Joystick5Button5:
                    case KeyCode.Joystick5Button6:
                    case KeyCode.Joystick5Button7:
                    case KeyCode.Joystick5Button8:
                    case KeyCode.Joystick5Button9:
                    case KeyCode.Joystick5Button10:
                    case KeyCode.Joystick5Button11:
                    case KeyCode.Joystick5Button12:
                    case KeyCode.Joystick5Button13:
                    case KeyCode.Joystick5Button14:
                    case KeyCode.Joystick5Button15:
                    case KeyCode.Joystick5Button16:
                    case KeyCode.Joystick5Button17:
                    case KeyCode.Joystick5Button18:
                    case KeyCode.Joystick5Button19:
                        return $"Joystick 5 Button {key - KeyCode.Joystick5Button0}";

                    //  Joystick 6 buttons
                    case KeyCode.Joystick6Button0:
                    case KeyCode.Joystick6Button1:
                    case KeyCode.Joystick6Button2:
                    case KeyCode.Joystick6Button3:
                    case KeyCode.Joystick6Button4:
                    case KeyCode.Joystick6Button5:
                    case KeyCode.Joystick6Button6:
                    case KeyCode.Joystick6Button7:
                    case KeyCode.Joystick6Button8:
                    case KeyCode.Joystick6Button9:
                    case KeyCode.Joystick6Button10:
                    case KeyCode.Joystick6Button11:
                    case KeyCode.Joystick6Button12:
                    case KeyCode.Joystick6Button13:
                    case KeyCode.Joystick6Button14:
                    case KeyCode.Joystick6Button15:
                    case KeyCode.Joystick6Button16:
                    case KeyCode.Joystick6Button17:
                    case KeyCode.Joystick6Button18:
                    case KeyCode.Joystick6Button19:
                        return $"Joystick 6 Button {key - KeyCode.Joystick6Button0}";

                    //  Joystick 7 buttons
                    case KeyCode.Joystick7Button0:
                    case KeyCode.Joystick7Button1:
                    case KeyCode.Joystick7Button2:
                    case KeyCode.Joystick7Button3:
                    case KeyCode.Joystick7Button4:
                    case KeyCode.Joystick7Button5:
                    case KeyCode.Joystick7Button6:
                    case KeyCode.Joystick7Button7:
                    case KeyCode.Joystick7Button8:
                    case KeyCode.Joystick7Button9:
                    case KeyCode.Joystick7Button10:
                    case KeyCode.Joystick7Button11:
                    case KeyCode.Joystick7Button12:
                    case KeyCode.Joystick7Button13:
                    case KeyCode.Joystick7Button14:
                    case KeyCode.Joystick7Button15:
                    case KeyCode.Joystick7Button16:
                    case KeyCode.Joystick7Button17:
                    case KeyCode.Joystick7Button18:
                    case KeyCode.Joystick7Button19:
                        return $"Joystick 7 Button {key - KeyCode.Joystick7Button0}";

                    //  Joystick 8 buttons
                    case KeyCode.Joystick8Button0:
                    case KeyCode.Joystick8Button1:
                    case KeyCode.Joystick8Button2:
                    case KeyCode.Joystick8Button3:
                    case KeyCode.Joystick8Button4:
                    case KeyCode.Joystick8Button5:
                    case KeyCode.Joystick8Button6:
                    case KeyCode.Joystick8Button7:
                    case KeyCode.Joystick8Button8:
                    case KeyCode.Joystick8Button9:
                    case KeyCode.Joystick8Button10:
                    case KeyCode.Joystick8Button11:
                    case KeyCode.Joystick8Button12:
                    case KeyCode.Joystick8Button13:
                    case KeyCode.Joystick8Button14:
                    case KeyCode.Joystick8Button15:
                    case KeyCode.Joystick8Button16:
                    case KeyCode.Joystick8Button17:
                    case KeyCode.Joystick8Button18:
                    case KeyCode.Joystick8Button19:
                        return $"Joystick 8 Button {key - KeyCode.Joystick8Button0}";
                    
                    default:
                        return key.ToString();
                }
            }

            public static string ToCharacter(KeyCode key) {
                switch (key) {
                    case KeyCode.Alpha0:
                    case KeyCode.Alpha1:
                    case KeyCode.Alpha2:
                    case KeyCode.Alpha3:
                    case KeyCode.Alpha4:
                    case KeyCode.Alpha5:
                    case KeyCode.Alpha6:
                    case KeyCode.Alpha7:
                    case KeyCode.Alpha8:
                    case KeyCode.Alpha9:
                        return $"{key - KeyCode.Alpha0}";

                    case KeyCode.UpArrow:       return "↑";
                    case KeyCode.DownArrow:     return "↓";
                    case KeyCode.LeftArrow:     return "←";
                    case KeyCode.RightArrow:    return "→";
                    default:
                        return key.ToString();
                }
            }
        }
    }
}

