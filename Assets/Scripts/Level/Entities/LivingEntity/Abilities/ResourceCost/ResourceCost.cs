public class ResourceCost {
    private readonly ResourceCostData data;
    private float resourceCostReduction = 0;

    public EntityDataType GetResourceType() => data.costType;

    public ResourceCost(LivingEntity owner, EntityDataType costType, int cost)
        : this(owner, new ResourceCostData(costType, cost)) {
    }

    public ResourceCost(LivingEntity owner, ResourceCostData data) {
        this.data = data;

        if (owner != null) {
            owner.GetAttributes().RegisterListener(AttributeTypes.ResourceCostReduction, ResourceCostReductionChanged);
            resourceCostReduction = owner.GetAttribute(AttributeTypes.ResourceCostReduction).GetValue() / 100f;
        }
    }

    public int GetCost() {
        float reduction = data.cost * resourceCostReduction;
        return (int) (data.cost - reduction);
    }

    public void ResourceCostReductionChanged(int param) {
        resourceCostReduction = (param / 1000f) / 100f;
    }
}

