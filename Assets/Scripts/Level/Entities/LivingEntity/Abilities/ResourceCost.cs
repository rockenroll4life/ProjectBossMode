public class ResourceCost{
    private readonly EntityDataType costType;
    private readonly int cost;
    private float resourceCostReduction = 0;

    public EntityDataType GetResourceType() => costType;

    public ResourceCost(LivingEntity owner, EntityDataType costType, int cost) {
        this.costType = costType;
        this.cost = cost;

        if (owner != null) {
            owner.GetAttributes().RegisterListener(AttributeTypes.ResourceCostReduction, ResourceCostReductionChanged);
            resourceCostReduction = owner.GetAttribute(AttributeTypes.ResourceCostReduction).GetValue() / 100f;
        }
    }

    public int GetCost() {
        float reduction = cost * resourceCostReduction;
        return (int) (cost - reduction);
    }

    public void ResourceCostReductionChanged(int param) {
        resourceCostReduction = (param / 1000f) / 100f;
    }
}

