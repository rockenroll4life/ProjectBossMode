using RockUtils.GameEvents;

//  Note: [Rock]: Currently this can only contain 20 elements
public enum EntityDataType {
    Health,
    Mana,

    _COUNT,
}

public class EntityData {
    System.Guid entityID;
    readonly float[] data = new float[(int) EntityDataType._COUNT];

    public EntityData(System.Guid entityID) {
        this.entityID = entityID;
    }

    public float Get(EntityDataType type) => data[(int) type];
    public void Set(EntityDataType type, float value) {
        data[(int) type] = value;
        EventManager.TriggerEvent(entityID, GameEvents.Entity_Data_Changed + (int) type, (int) (value * 1000));
    }
}
