using UnityEngine;

public class Settings : MonoBehaviour {
    static Settings instance;

    readonly KeyBindings keyBindings = new KeyBindings();

    public static Settings Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Settings>();
                if (instance == null) {
                    GameObject singleton = new GameObject("Settings");
                    instance = singleton.AddComponent<Settings>();
                    instance.Setup();
                }
            }
            return instance;
        }
    }

    public static KeyCode GetKeyBinding(KeyBindingKeys key) => Instance.keyBindings.GetKeyBinding(key);

    private void Awake() {
        if (instance == null) {
            instance = this;
            Setup();
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of Settings found. Destroying this instance.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Setup() {
        keyBindings.Setup();
        //  TODO: [Rock]: Load the settings
    }
}
