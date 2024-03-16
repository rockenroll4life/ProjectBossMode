using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;
using RockUtils.StreamingAssetsUtils;
using RockUtils.CSVReaderUtils;
using RockUtils.ParseUtils;

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

    _COUNT,
}

public enum KeyBindingType {
    Keyboard,
    Controller,
}

public class KeyBindings {
    static readonly string[] KEYBINDING_LOCATIONS = new string[] {
            "Settings/Default_Keyboard_KeyBindings.csv",
            "Settings/Default_Keyboard_ControllerBindings.csv"
    };
    static readonly int SAVE_TABLE_SIZE = 2;

    readonly Dictionary<KeyBindingType, Dictionary<KeyBindingKeys, KeyCode>> DEFAULT_BINDINGS = new Dictionary<KeyBindingType, Dictionary<KeyBindingKeys, KeyCode>>() {
        { KeyBindingType.Keyboard, new Dictionary<KeyBindingKeys, KeyCode>() },
        { KeyBindingType.Controller, new Dictionary<KeyBindingKeys, KeyCode>() }
    };

    readonly Dictionary<KeyBindingType, Dictionary<KeyBindingKeys, KeyCode>> bindings = new Dictionary<KeyBindingType, Dictionary<KeyBindingKeys, KeyCode>>() {
        { KeyBindingType.Keyboard, new Dictionary<KeyBindingKeys, KeyCode>() },
        { KeyBindingType.Controller, new Dictionary<KeyBindingKeys, KeyCode>() }
    };

    public KeyCode GetKeyBinding(KeyBindingKeys keyBindingKey) => bindings[KeyBindingType.Keyboard][keyBindingKey];

    public void Setup() {
        foreach (KeyBindingType type in DEFAULT_BINDINGS.Keys) {
            LoadDefaultCSV(type, KEYBINDING_LOCATIONS[(int) type]);

            //  Check to see if we have an existing key bindings settings file on file..
            if (SavedSettingsExist(type)) {
                LoadSavedSettings(type);
                EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
            }
            //  If we don't, we want to load up our keybindings as the default one
            else {
                ResetKeybindings(type, false);
            }
        }

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }

    bool SavedSettingsExist(KeyBindingType keybindingType) {
        //  TODO: [Rock]: Actually check if the file exist
        return false;
    }

    void LoadSavedSettings(KeyBindingType keybindingType) {
        //  TODO: [Rock]: Loading up the keybindings saved settings from file
    }

    void LoadDefaultCSV(KeyBindingType keyBindingType, string settingsLocation) {
        TextAsset keyBindingText = StreamingAssetsUtils.LoadTextAsset(settingsLocation);
        string[] keyBindings = CSVReaderUtils.ReadCSV(keyBindingText, SAVE_TABLE_SIZE);
        var defaultData = DEFAULT_BINDINGS[keyBindingType];

        int tableSize = keyBindings.Length / SAVE_TABLE_SIZE;
        for (int i = 0; i < tableSize; i++) {
            int offset = (i * SAVE_TABLE_SIZE);
            KeyBindingKeys keyBindingsKey = ParseUtils.Parse<KeyBindingKeys>(keyBindings[offset + 0]);
            KeyCode keyCode = ParseUtils.Parse<KeyCode>(keyBindings[offset + 1]);
            defaultData.Add(keyBindingsKey, keyCode);
        }
    }

    public void ResetKeybindings(KeyBindingType keyBindingType, bool triggerEvent = true) {
        var defaultData = DEFAULT_BINDINGS[keyBindingType];
        foreach (KeyBindingKeys key in defaultData.Keys) {
            bindings[keyBindingType][key] = defaultData[key];
        }

        if (triggerEvent) {
            EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
        }
    }

    void ResetAllKeybindings() {
        foreach (KeyBindingType type in DEFAULT_BINDINGS.Keys) {
            ResetKeybindings(type, false);
        }

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }
}
