using UnityEngine;
using RockUtils.StreamingAssetsUtils;

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
                return StreamingAssetsUtils.LoadSprite("Abilities/" + data.iconName, 512, 512);
            }
        }

        return null;
    }
}
