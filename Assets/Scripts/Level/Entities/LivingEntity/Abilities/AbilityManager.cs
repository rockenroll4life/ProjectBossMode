using UnityEngine;
using RockUtils.StreamingAssetsUtils;
using RockUtils.CSVReaderUtils;
using RockUtils.ParseUtils;
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

    [SerializeField]
    private readonly Dictionary<Ability.ID, AbilityData> abilityData = new Dictionary<Ability.ID, AbilityData>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of AbilityManager found. Destroying this instance.");
            Destroy(gameObject);
        }

        LoadCSV();
    }

    void LoadCSV() {
        TextAsset abilityText = StreamingAssetsUtils.LoadTextAsset($"Abilities/Abilities.csv");
        string[] abilities = CSVReaderUtils.ReadCSV(abilityText, 5);

        int tableSize = abilities.Length / 5;
        for (int i = 0; i < tableSize; i++) {
            Ability.ID id = ParseUtils.Parse<Ability.ID>(abilities[(i * tableSize) + 0]);
            AbilityData newData = new AbilityData(abilities[(i * tableSize) + 0], abilities[(i * tableSize) + 1], abilities[(i * tableSize) + 2], abilities[(i * tableSize) + 3], abilities[(i * tableSize) + 4]);
            abilityData.Add(id, newData);
        }
    }

    public static Sprite GetAbilityIcon(Ability.ID abilityID) {
        if (instance.abilityData.TryGetValue(abilityID, out AbilityData data)) {
            return StreamingAssetsUtils.LoadSprite($"Abilities/Icons/{data.iconName}.png", 512, 512);
        }

        return null;
    }

    public static ResourceCostData GetResourceCostData(Ability.ID abilityID) {
        if (instance.abilityData.TryGetValue(abilityID, out AbilityData data)) {
            return data.resourceCost;
        }

        return Ability.Info.FREE_RESOURCE_COST.GetResourceCostData();
    }

    public static AbilityData GetAbilityData(Ability.ID abilityID) {
        if (instance.abilityData.TryGetValue(abilityID, out AbilityData data)) {
            return data;
        }

        return null;
    }
}
