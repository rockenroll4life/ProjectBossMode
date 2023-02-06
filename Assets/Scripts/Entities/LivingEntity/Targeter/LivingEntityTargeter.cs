using UnityEngine;

public class LivingEntityTargeter : TargeterBase {
    protected LivingEntity targetedEntity = null;
    protected Vector3? targetedLocation = null;

    public LivingEntityTargeter(LivingEntity owner)
        : base(owner) {
    }

    public override LivingEntity GetTargetedEntity() {
        return targetedEntity;
    }

    public override Vector3? GetTargetedLocation() {
        return targetedLocation;
    }
}
