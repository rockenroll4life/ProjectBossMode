using UnityEngine;
using RockUtils.GameEvents;

public class TargeterBase : ITargeter {
    public enum TargetType {
        None,
        World,
        LivingEntity,
        Mob,
        Player,
        Interactable,
    }

    protected IDamageable targetedEntity = null;
    protected Vector3? targetedLocation = null;

    protected static readonly int RAYCAST_DISTANCE = 100;

    protected readonly LivingEntity owner;
    
    public TargeterBase(LivingEntity owner) {
        this.owner = owner;
    }

    public void SetTargetedEntity(IDamageable entity) {
        targetedEntity = entity;

        if (entity != null) {
            EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Targeted_Entity);
        }
    }
    public void SetTargetedLocation(Vector3? location) {
        targetedLocation = location;

        if (location.HasValue) {
            EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Targeted_World);
        }
    }

    public IDamageable GetTargetedEntity() { return targetedEntity; }
    public Vector3? GetTargetedLocation() { return targetedLocation; }
    
    public virtual void Update() { }

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
