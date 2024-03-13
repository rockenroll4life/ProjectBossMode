[System.Serializable]
public class AbilityData {
    public Ability.ID id;
    public string displayName;
    public string iconName;
    public ResourceCostData resourceCost;

    public AbilityData(string id, string displayName, string iconName, string costType, string cost) {
        this.id = (Ability.ID) System.Enum.Parse(typeof(Ability.ID), id);
        this.displayName = displayName;
        this.iconName = iconName;
        resourceCost = new ResourceCostData((EntityDataType) System.Enum.Parse(typeof(EntityDataType), costType), int.Parse(cost));
    }
}
