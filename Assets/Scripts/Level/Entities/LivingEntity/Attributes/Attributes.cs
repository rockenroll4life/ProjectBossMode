using System.Collections.Generic;

public enum AttributeTypes {
    None = -1,
    HealthMax,
    HealthRegenRate,

    ManaMax,
    ManaRegenRate,

    MovementSpeed,

    AttackDamage,
    AttackSpeed,
    AttackRange,

    Ability1Cooldown,
    Ability2Cooldown,
    Ability3Cooldown,
    Ability4Cooldown,
    UltimateCooldown,

    CooldownReduction,
    ResourceCostReduction,
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
        { AttributeTypes.AttackRange, new("generic.attackRange", 5, 0, float.MaxValue) },

        { AttributeTypes.Ability1Cooldown, new("generic.ability1", 5, 0, float.MaxValue) },
        { AttributeTypes.Ability2Cooldown, new("generic.ability2", 5, 0, float.MaxValue) },
        { AttributeTypes.Ability3Cooldown, new("generic.ability3", 5, 0, float.MaxValue) },
        { AttributeTypes.Ability4Cooldown, new("generic.ability4", 5, 0, float.MaxValue) },
        { AttributeTypes.UltimateCooldown, new("generic.ultimate", 5, 0, float.MaxValue) },

        { AttributeTypes.CooldownReduction, new("generic.cooldownReduction", 0, float.MinValue, 100) },
        { AttributeTypes.ResourceCostReduction, new("generic.resourceCostReduction", 0, float.MinValue, 100) },
    };

    public static RangedAttribute Get(AttributeTypes type) => attribute[type];
}
