using System.Collections.Generic;
using RockUtils.GameEvents;

public enum GameplayOptions {
    RotateTowardsMouse,
}

public class GameplaySettings {
    static readonly Dictionary<GameplayOptions, int> DEFAULT_BINDINGS = new() {
        { GameplayOptions.RotateTowardsMouse, 1 }
    };

    readonly Dictionary<GameplayOptions, int> bindings = new(DEFAULT_BINDINGS);

    public int GetGameplaySetting(GameplayOptions option) => bindings[option];

    public void ResetGameplaySettings() {
        foreach (GameplayOptions option in DEFAULT_BINDINGS.Keys) {
            bindings[option] = DEFAULT_BINDINGS[option];
        }

        EventManager.TriggerEvent((int) GameEvents.GameplaySettings_Changed);
    }

    public void Setup() {
        //  TODO: [Rock]: Load the keybindings from our config

        EventManager.TriggerEvent((int) GameEvents.GameplaySettings_Changed);
    }
}
