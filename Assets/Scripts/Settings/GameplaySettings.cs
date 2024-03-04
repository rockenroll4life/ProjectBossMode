using System.Collections.Generic;

public enum GameplayOptions {
    RotateTowardsMouse,
}

public class GameplaySettings {
    static readonly Dictionary<GameplayOptions, int> DEFAULT_BINDINGS = new() {
        { GameplayOptions.RotateTowardsMouse, 1 }
    };

    readonly Dictionary<GameplayOptions, int> bindings = new(DEFAULT_BINDINGS);

    public int GetGameplaySetting(GameplayOptions option) => bindings[option];

    public void Setup() {
        //  TODO: [Rock]: Load the keybindings from our config
    }
}
