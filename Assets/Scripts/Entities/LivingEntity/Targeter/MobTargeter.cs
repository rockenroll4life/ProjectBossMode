using UnityEngine;

public class MobTargeter : TargeterBase {
    readonly LivingEntity targetEntity;

    public MobTargeter(LivingEntity owner)
        : base(owner) {

        targetEntity = Object.FindObjectOfType<Player>();
    }

    public override LivingEntity GetTargetedEntity() { return targetEntity; }

    public override Vector3? GetTargetedLocation() { return null; }
}
