using System.Collections.Generic;
using UnityEngine;

public class PatrolGoal : Goal {
    readonly LivingEntity owner;
    readonly Vector3[] randPos = new Vector3[2];
    int currentTarget = 0;

    public PatrolGoal(LivingEntity owner) {
        this.owner = owner;
        randPos[0] = new Vector3(-5, 0, 6);
        randPos[1] = new Vector3(5, 0, 6);

        SetFlags(new HashSet<Flag>() { Flag.MOVE });
    }

    public override bool CanUse() {
        return true;
    }

    public override void Start() {
        base.Start();

        owner.GetTargeter().SetTargetedLocation(randPos[currentTarget]);
    }

    public override void Stop() {
        base.Stop();

        owner.GetTargeter().SetTargetedLocation(null);
    }

    public override void Update() {
        base.Update();

        float dist = Vector3.Distance(owner.transform.position, randPos[currentTarget]);
        if (dist <= 0.5f) {
            currentTarget = (currentTarget + 1) % 2;
            owner.GetTargeter().SetTargetedLocation(randPos[currentTarget]);
        }
    }
}
