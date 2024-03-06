using System;

public class ResourceCost{
    private readonly EntityDataType costType;
    private readonly int cost;

    public EntityDataType GetResourceType() => costType;

    public ResourceCost(EntityDataType costType, int cost) {
        this.costType = costType;
        this.cost = cost;
    }

    public int GetCost(LivingEntity owner) {
        //  TODO: [Rock]: Incorporate mana cost reduction from attributes
        return cost;
    }
}

