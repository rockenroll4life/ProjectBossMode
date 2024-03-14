using RockUtils.ParseUtils;

[System.Serializable]
public class AbilityData {
    public Ability.ID id;
    public string displayName;
    public string iconName;
    public ResourceCostData resourceCost;

    public AbilityData(string id, string displayName, string iconName, string costType, string cost) {
        this.id = ParseUtils.Parse<Ability.ID>(id);
        this.displayName = displayName;
        this.iconName = iconName;
        resourceCost = new ResourceCostData(ParseUtils.Parse<EntityDataType>(costType), ParseUtils.Parse<int>(cost));
    }
}
