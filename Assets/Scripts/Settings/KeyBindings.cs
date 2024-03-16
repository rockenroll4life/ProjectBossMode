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

public class KeyBindings {
    private static readonly int SAVE_TABLE_SIZE = 2;

    readonly Dictionary<KeyBindingKeys, KeyCode> DEFAULT_BINDINGS = new Dictionary<KeyBindingKeys, KeyCode>();
    readonly Dictionary<KeyBindingKeys, KeyCode> bindings = new Dictionary<KeyBindingKeys, KeyCode>();
    
    public KeyCode GetKeyBinding(KeyBindingKeys keyBindingKey) => bindings[keyBindingKey];

    public void ResetKeybindings() {
        foreach (KeyBindingKeys key in DEFAULT_BINDINGS.Keys) {
            bindings[key] = DEFAULT_BINDINGS[key];
        }

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }

    public void Setup() {
        LoadDefaultCSV();

        //  Check to see if we have an existing key bindings settings file on file..
        if (SavedSettingsExist()) {
            LoadSavedSettings();
            EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
        }
        //  If we don't, we want to load up our keybindings as the default one
        else {
            ResetAllKeybindings();
        }

    }

    bool SavedSettingsExist() {
        //  TODO: [Rock]: Actually check if the file exist
        return false;
    }

    void LoadSavedSettings() {
        //  TODO: [Rock]: Loading up the keybindings saved settings from file
    }

    void LoadDefaultCSV() {
        TextAsset keyBindingText = StreamingAssetsUtils.LoadTextAsset($"Settings/Default_KeyBindings.csv");
        string[] keyBindings = CSVReaderUtils.ReadCSV(keyBindingText, SAVE_TABLE_SIZE);

        int tableSize = keyBindings.Length / SAVE_TABLE_SIZE;
        for (int i = 0; i < tableSize; i++) {
            int offset = (i * SAVE_TABLE_SIZE);
            KeyBindingKeys keyBindingsKey = ParseUtils.Parse<KeyBindingKeys>(keyBindings[offset + 0]);
            KeyCode keyCode = ParseUtils.Parse<KeyCode>(keyBindings[offset + 1]);
            DEFAULT_BINDINGS.Add(keyBindingsKey, keyCode);
        }
    }

    void ResetAllKeybindings() {
        bindings.Clear();

        //  If we don't, we want to load up our keybindings as the default one
        foreach (KeyBindingKeys key in DEFAULT_BINDINGS.Keys) {
            bindings.Add(key, DEFAULT_BINDINGS[key]);
        }

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }
}
