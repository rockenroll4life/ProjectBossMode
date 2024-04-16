using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;
using RockUtils.StreamingAssetsUtils;
using RockUtils.CSVReaderUtils;
using RockUtils.ParseUtils;
using RockUtils.JsonUtils;
using System.IO;
using static RockUtils.GameEvents.InputManager;

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

[System.Serializable]
public class KeyBinding {
    public readonly KeyCode keyboard;
    public readonly ControllerButtons controller;

    public KeyBinding(KeyCode keyboard, ControllerButtons controller) {
        this.keyboard = keyboard;
        this.controller = controller;
    }

    public bool IsBinding(KeyCode code) => code == keyboard;
    public bool IsBinding(ControllerButtons button) => button == controller;
}

public class KeyBindings {
    static readonly string DIRECTORY_NAME =  "Settings";
    static readonly string KEYBINDING_DEFAULT_LOCATION = "Settings/Default_KeyBindings.csv";
    static readonly string KEYBINDING_SAVE = "Settings/KeyBindings.json";
    static readonly int SAVE_TABLE_SIZE = 3;

    readonly Dictionary<KeyBindingKeys, KeyBinding> DEFAULT_BINDINGS = new Dictionary<KeyBindingKeys, KeyBinding>();
    Dictionary<KeyBindingKeys, KeyBinding> bindings = new Dictionary<KeyBindingKeys, KeyBinding>();

    string GetDirectoryPath() => Application.persistentDataPath + Path.DirectorySeparatorChar + DIRECTORY_NAME;
    string GetFilePath() => Application.persistentDataPath + Path.DirectorySeparatorChar + KEYBINDING_SAVE;

    public KeyBinding GetKeyBinding(KeyBindingKeys keyBindingKey) => bindings[keyBindingKey];

    public void Setup() {
        CheckForSettingsFolder();

        LoadDefaultCSV();

        //  Check to see if we have an existing key bindings settings file on file..
        if (SavedSettingsExist()) {
            LoadSettings();
            EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
        }
        //  If we don't, we want to load up our keybindings as the default one
        else {
            ResetAllKeybindings();
        }

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }

    void CheckForSettingsFolder() {
        if (!Directory.Exists(GetDirectoryPath())) {
            Directory.CreateDirectory(GetDirectoryPath());
        }
    }

    bool SavedSettingsExist() {
        return File.Exists(GetFilePath());
    }

    void SaveSettings() {
        JsonUtils.SaveData(bindings, GetFilePath());
    }

    void LoadSettings() {
        bindings = JsonUtils.LoadData<Dictionary<KeyBindingKeys, KeyBinding>>(GetFilePath());
    }

    void LoadDefaultCSV() {
        TextAsset keyBindingText = StreamingAssetsUtils.LoadTextAsset(KEYBINDING_DEFAULT_LOCATION);
        string[] keyBindings = CSVReaderUtils.ReadCSV(keyBindingText, SAVE_TABLE_SIZE);

        int tableSize = keyBindings.Length / SAVE_TABLE_SIZE;
        for (int i = 0; i < tableSize; i++) {
            int offset = (i * SAVE_TABLE_SIZE);
            KeyBindingKeys keyBindingsKey = ParseUtils.Parse<KeyBindingKeys>(keyBindings[offset + 0]);
            KeyCode keyboard = ParseUtils.Parse<KeyCode>(keyBindings[offset + 1]);
            ControllerButtons controller = ParseUtils.Parse<ControllerButtons>(keyBindings[offset + 2]);
            DEFAULT_BINDINGS.Add(keyBindingsKey, new KeyBinding(keyboard, controller));
        }
    }

    public void ResetKeybinding(KeyBindingKeys keyBindingKeys, bool triggerEvent = true) {
        bindings[keyBindingKeys] = DEFAULT_BINDINGS[keyBindingKeys];

        if (triggerEvent) {
            EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
        }
    }

    void ResetAllKeybindings() {
        foreach(KeyBindingKeys key in DEFAULT_BINDINGS.Keys) {
            ResetKeybinding(key, false);
        }

        JsonUtils.SaveData(DEFAULT_BINDINGS, GetFilePath());

        EventManager.TriggerEvent(GameEvents.Keybindings_Changed);
    }
}
