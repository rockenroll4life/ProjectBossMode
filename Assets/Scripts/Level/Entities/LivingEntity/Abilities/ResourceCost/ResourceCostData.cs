[System.Serializable]
public class ResourceCostData {
    public EntityDataType costType;
    public int cost;

    public ResourceCostData(EntityDataType costType, int cost) {
        this.costType = costType;
        this.cost = cost;
    }
}
