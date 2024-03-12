using UnityEngine;

public class AbilityManager : MonoBehaviour {
    static AbilityManager instance;

    public static AbilityManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<AbilityManager>();
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of AbilityManager found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private AbilityData[] abilityData;

    public static Sprite GetAbilityIcon(Ability.ID abilityID) {
        foreach (AbilityData data in instance.abilityData) {
            if (data.id == abilityID) {
                string filePath = Application.streamingAssetsPath + "/Abilities/" + data.iconName;
                byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);

                Texture2D tex = new Texture2D(512, 512);
                tex.LoadImage(pngBytes);

                return Sprite.Create(tex, new Rect(0f, 0f, 512, 512), new Vector2(0.5f, 0.5f), 100f);
            }
        }

        return null;
    }
}
