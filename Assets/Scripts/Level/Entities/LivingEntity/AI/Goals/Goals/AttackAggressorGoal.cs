using System.Collections.Generic;

public class AttackAggressorGoal : Goal {
    readonly LivingEntity owner;

    public AttackAggressorGoal(LivingEntity owner) {
        this.owner = owner;

        SetFlags(new HashSet<Flag>() { Flag.MOVE });
    }

    public override bool CanUse() => owner.GetLastDamager() != null;

    public override void Start() {
        base.Start();

        if (owner.GetLastDamager() is IDamageable damagable) {
            owner.GetTargeter().SetTargetedEntity(damagable);
        }
    }

    public override void Stop() {
        base.Stop();

        owner.GetTargeter().SetTargetedEntity(null);
    }
}
