public class LivingEntitySharedAttributes {
    //  Some generic values for these stats, they can be adjusted on a per entity basis
    public static readonly RangedAttribute MAX_HEALTH = new("generic.health", 100, 0, float.MaxValue);
    public static readonly RangedAttribute MOVEMENT_SPEED = new("generic.movementSpeed", 13.2f, 0, float.MaxValue);
    public static readonly RangedAttribute ATTACK_DAMAGE = new("generic.attackDamage", 25, 0, float.MaxValue);

    //  NOTE: [Rock]: We may need to format attribute values for saving...we'll save that for when we need it
}
