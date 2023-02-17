using System.Collections.Generic;

public class AttackAggressorGoal : Goal {
    readonly LivingEntity owner;

    public AttackAggressorGoal(LivingEntity owner) {
        this.owner = owner;

        SetFlags(new HashSet<Flag>() { Flag.MOVE });
    }

    public override bool CanUse() {
        return owner.GetLastDamager() != null;
    }

    public override void Start() {
        base.Start();

        owner.GetTargeter().SetTargetedEntity(owner.GetLastDamager());
    }

    public override void Stop() {
        base.Stop();

        owner.GetTargeter().SetTargetedEntity(null);
    }
}
