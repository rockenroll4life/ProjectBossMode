using UnityEngine;
using RockUtils.StreamingAssetsUtils;
using RockUtils.CSVReaderUtils;
using RockUtils.ParseUtils;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {
    private static readonly int SAVE_TABLE_SIZE = 5;
    static AbilityManager instance;

    public static AbilityManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<AbilityManager>();
            }
            return instance;
        }
    }

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
        string[] abilities = CSVReaderUtils.ReadCSV(abilityText, SAVE_TABLE_SIZE);

        int tableSize = abilities.Length / SAVE_TABLE_SIZE;
        for (int i = 0; i < tableSize; i++) {
            int offset = (i * SAVE_TABLE_SIZE);
            Ability.ID id = ParseUtils.Parse<Ability.ID>(abilities[offset + 0]);
            AbilityData newData = new AbilityData(abilities[offset + 0], abilities[offset + 1], abilities[offset + 2], abilities[offset + 3], abilities[offset + 4]);
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
