using System.Collections.Generic;

public class AttackTower : Goal {
    readonly LivingEntity owner;
    readonly Tower tower;

    public AttackTower(LivingEntity owner) {
        this.owner = owner;

        tower = owner.GetLevel().GetEntityManager().GetFirstEntityOfType<Tower>();

        SetFlags(new HashSet<Flag>() { Flag.MOVE });
    }

    public override bool CanUse() => tower != null;

    public override void Start() {
        base.Start();

        owner.GetTargeter().SetTargetedEntity(tower);
    }

    public override void Stop() {
        base.Stop();

        owner.GetTargeter().SetTargetedEntity(null);
    }
}
