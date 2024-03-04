public class LivingEntitySharedAttributes {
    //  Some generic values for these stats, they can be adjusted on a per entity basis
    //  TODO: [Rock]: We should make these ranges realistic
    public static readonly RangedAttribute HEALTH_MAX = new("generic.health", 100, 0, float.MaxValue);
    public static readonly RangedAttribute HEALTH_REGEN_RATE = new("generic.healthRegenRate", 1f, float.MinValue, float.MaxValue);

    public static readonly RangedAttribute MANA_MAX = new("generic.mana", 0, 0, float.MaxValue);
    public static readonly RangedAttribute MANA_REGEN_RATE = new("generic.manaRegenRate", 2.5f, float.MinValue, float.MaxValue);

    public static readonly RangedAttribute MOVEMENT_SPEED = new("generic.movementSpeed", 3.5f, 0, float.MaxValue);

    public static readonly RangedAttribute ATTACK_DAMAGE = new("generic.attackDamage", 25, 0, float.MaxValue);
    public static readonly RangedAttribute ATTACK_SPEED = new("generic.attackSpeed", 3, 0, float.MaxValue);
    public static readonly RangedAttribute ATTACK_RANGE = new("generic.attackRange", 5, 0, float.MaxValue);

    //  NOTE: [Rock]: We may need to format attribute values for saving/loading...we'll save that for when we need it
}
