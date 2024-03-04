using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;

public enum KeyBindingKeys {
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,

    Ability1,
    Ability2,
    Ability3,
    Ability4,
    Ultimate,
    Dash,

    _COUNT,
}

public class KeyBindings {
    static readonly Dictionary<KeyBindingKeys, KeyCode> DEFAULT_BINDINGS = new() {
        { KeyBindingKeys.MoveUp, KeyCode.W },
        { KeyBindingKeys.MoveDown, KeyCode.S },
        { KeyBindingKeys.MoveLeft, KeyCode.A },
        { KeyBindingKeys.MoveRight, KeyCode.D },

        { KeyBindingKeys.Ability1, KeyCode.Alpha1 },
        { KeyBindingKeys.Ability2, KeyCode.Alpha2 },
        { KeyBindingKeys.Ability3, KeyCode.Alpha3 },
        { KeyBindingKeys.Ability4, KeyCode.Alpha4 },
        { KeyBindingKeys.Ultimate, KeyCode.Alpha5 },
        { KeyBindingKeys.Dash, KeyCode.LeftShift }
    };

    readonly Dictionary<KeyBindingKeys, KeyCode> bindings = new(DEFAULT_BINDINGS);
    
    public KeyCode GetKeyBinding(KeyBindingKeys keyBindingKey) => bindings[keyBindingKey];

    public void ResetKeybindings() {
        foreach (KeyBindingKeys key in DEFAULT_BINDINGS.Keys) {
            bindings[key] = DEFAULT_BINDINGS[key];
        }

        EventManager.TriggerEvent((int) GameEvents.Keybindings_Changed);
    }

    public void Setup() {
        //  TODO: [Rock]: Load the keybindings from our config

        EventManager.TriggerEvent((int) GameEvents.Keybindings_Changed);
    }

}
