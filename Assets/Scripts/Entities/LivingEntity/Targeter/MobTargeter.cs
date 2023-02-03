using UnityEngine;

public class MobTargeter : TargeterBase {
    public MobTargeter(LivingEntity owner)
        : base(owner) {
    }

    public override LivingEntity GetTargetedEntity() { return null; }

    public override Vector3? GetTargetedLocation() { return null; }
}
