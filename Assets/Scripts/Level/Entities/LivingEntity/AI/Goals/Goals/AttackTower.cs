using System.Collections.Generic;

public class AttackTower : Goal {
    readonly LivingEntity owner;
    Tower tower;

    public AttackTower(LivingEntity owner) {
        this.owner = owner;

        SetFlags(new HashSet<Flag>() { Flag.MOVE });
    }

    public override bool CanUse() {
        if (tower == null) {
            tower = owner.GetLevel().EntityManager.GetFirstEntityOfType<Tower>();
        }
        
        return tower != null;
    }

    public override void Start() {
        base.Start();

        owner.GetTargeter().SetTargetedEntity(tower);
    }

    public override void Stop() {
        base.Stop();

        owner.GetTargeter().SetTargetedEntity(null);
    }
}
