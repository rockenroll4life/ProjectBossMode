using UnityEngine;

public abstract class TargeterBase : Targeter {
    public enum TargetType {
        None,
        World,
        LivingEntity,
        Mob,
        Player,
        Interactable,
    }

    protected static readonly int RAYCAST_DISTANCE = 100;

    protected readonly LivingEntity owner;
    
    public TargeterBase(LivingEntity owner) {
        this.owner = owner;
    }

    public abstract LivingEntity GetTargetedEntity();

    public abstract Vector3? GetTargetedLocation();

    protected TargetType EntityTypeToTargetType(Entity.EntityType entityType) {
        switch (entityType) {
            case Entity.EntityType.LivingEntity:    return TargetType.LivingEntity;
            case Entity.EntityType.Player:          return TargetType.Player;
            case Entity.EntityType.Mob:             return TargetType.Mob;
            case Entity.EntityType.Interactable:    return TargetType.Interactable;

            default:                                return TargetType.None;
        }
    }
}
