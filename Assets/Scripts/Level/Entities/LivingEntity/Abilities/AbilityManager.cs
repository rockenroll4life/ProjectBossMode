using UnityEngine;
using RockUtils.StreamingAssetsUtils;
using System.Collections.Generic;

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
    private readonly Dictionary<Ability.ID, AbilityData> _abilityData = new Dictionary<Ability.ID, AbilityData>();

    public static Sprite GetAbilityIcon(Ability.ID abilityID) {
        foreach (AbilityData data in instance.abilityData) {
            if (data.id == abilityID) {
                return StreamingAssetsUtils.LoadSprite($"Abilities/{data.iconName}.png", 512, 512);
            }
        }

        return null;
    }

    public static Sprite _GetAbilityIcon(Ability.ID abilityID) {
        return StreamingAssetsUtils.LoadSprite($"Abilities/{instance._abilityData[abilityID].iconName}.png", 512, 512);
    }

    public static ResourceCostData GetResourceCostData(Ability.ID abilityID) {
        foreach (AbilityData data in instance.abilityData) {
            if (data.id == abilityID) {
                return data.resourceCost;
            }
        }

        return Ability.Info.FREE_RESOURCE_COST.GetResourceCostData();
    }

    public static ResourceCostData _GetResourceCostData(Ability.ID abilityID) {
        if (instance._abilityData.TryGetValue(abilityID, out AbilityData data)) {
            return data.resourceCost;
        }

        return Ability.Info.FREE_RESOURCE_COST.GetResourceCostData();
    }

    public static AbilityData GetAbilityData(Ability.ID abilityID) {
        foreach (AbilityData data in instance.abilityData) {
            if (data.id == abilityID) {
                return data;
            }
        }

        return null;
    }

    public static AbilityData _GetAbilityData(Ability.ID abilityID) {
        return instance._abilityData[abilityID];
    }
}
