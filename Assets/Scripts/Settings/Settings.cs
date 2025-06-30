using UnityEngine;

public class Settings : Singleton<Settings> {
    readonly KeyBindings keyBindings = new KeyBindings();
    readonly GameplaySettings gameplaySettings = new GameplaySettings();

    public static KeyBinding GetKeyBinding(KeyBindingKeys key) => Instance.keyBindings.GetKeyBinding(key);
    public static int GetGameplaySetting(GameplayOptions option) => Instance.gameplaySettings.GetGameplaySetting(option);

    protected override void Awake() {
        base.Awake();

        Setup();
    }

    void Setup() {
        keyBindings.Setup();
        gameplaySettings.Setup();
        //  TODO: [Rock]: Load the settings
    }
}
