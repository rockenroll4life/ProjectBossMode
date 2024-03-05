using System.Collections.Generic;

public enum AttributeTypes {
    HealthMax,
    HealthRegenRate,

    ManaMax,
    ManaRegenRate,

    MovementSpeed,

    AttackDamage,
    AttackSpeed,
    AttackRange,
}

public class Attributes {
    //  Some generic values for these stats, they can be adjusted on a per entity basis
    static readonly Dictionary<AttributeTypes, RangedAttribute> attribute = new Dictionary<AttributeTypes, RangedAttribute> {
        { AttributeTypes.HealthMax, new("generic.health", 100, 0, float.MaxValue) },
        { AttributeTypes.HealthRegenRate, new("generic.healthRegenRate", 1f, float.MinValue, float.MaxValue) },

        { AttributeTypes.ManaMax, new("generic.mana", 0, 0, float.MaxValue) },
        { AttributeTypes.ManaRegenRate, new("generic.manaRegenRate", 2.5f, float.MinValue, float.MaxValue) },

        { AttributeTypes.MovementSpeed, new("generic.movementSpeed", 3.5f, 0, float.MaxValue) },

        { AttributeTypes.AttackDamage, new("generic.attackDamage", 25, 0, float.MaxValue) },
        { AttributeTypes.AttackSpeed, new("generic.attackSpeed", 3, 0, float.MaxValue) },
        { AttributeTypes.AttackRange, new("generic.attackRange", 5, 0, float.MaxValue) }
    };

    public static RangedAttribute Get(AttributeTypes type) => attribute[type];
}
