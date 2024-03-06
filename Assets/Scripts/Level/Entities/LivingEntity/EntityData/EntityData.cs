using RockUtils.GameEvents;

//  Note: [Rock]: Currently this can only contain 20 elements
public enum EntityDataType {
    Health,
    Mana,

    Ability1_Cooldown,
    Ability2_Cooldown,
    Ability3_Cooldown,
    Ability4_Cooldown,
    Ultimate_Cooldown,

    _COUNT,
}

public class EntityData {
    System.Guid entityID;
    readonly IEntityData[] data;

    public EntityData(LivingEntity owner) {
        entityID = owner.GetEntityID();

        data = new IEntityData[] {
            new UpdatingEntityData(owner, EntityDataType.Health, owner.GetAttribute(AttributeTypes.HealthMax).GetValue(), AttributeTypes.HealthMax, AttributeTypes.HealthRegenRate),
            new UpdatingEntityData(owner, EntityDataType.Mana, owner.GetAttribute(AttributeTypes.ManaMax).GetValue(), AttributeTypes.ManaMax, AttributeTypes.ManaRegenRate),

            new UpdatingEntityData(owner, EntityDataType.Ability1_Cooldown, 0, AttributeTypes.Ability1Cooldown),
            new UpdatingEntityData(owner, EntityDataType.Ability2_Cooldown, 0, AttributeTypes.Ability2Cooldown),
            new UpdatingEntityData(owner, EntityDataType.Ability3_Cooldown, 0, AttributeTypes.Ability3Cooldown),
            new UpdatingEntityData(owner, EntityDataType.Ability4_Cooldown, 0, AttributeTypes.Ability4Cooldown),
            new UpdatingEntityData(owner, EntityDataType.Ultimate_Cooldown, 0, AttributeTypes.UltimateCooldown),
        };
    }

    public float Get(EntityDataType type) => data[(int) type].Get();
    public void Set(EntityDataType type, float value) {
        data[(int) type].Set(value);
        EventManager.TriggerEvent(entityID, GameEvents.Entity_Data_Changed + (int) type, (int) (value * 1000));
    }

    public void Update() {
        foreach (IEntityData item in data) {
            item.Update();
        }
    }
}
