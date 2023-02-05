using UnityEngine;

public class MobLocomotion : LivingEntityLocomotion {
    readonly Vector3[] randPos = new Vector3[2];
    int currentTarget = 0;

    public MobLocomotion(LivingEntity owner)
        : base(owner) {

        randPos[0] =  new Vector3(-5, 0, 6);
        randPos[1] = new Vector3(5, 0, 6);
    }

    public override void Update() {
        base.Update();

        //  NOTE: [Rock]: Code like this will want to be moved out of here and into the targeter...
        if (!targetedEntity) {
            float dist = Vector3.Distance(owner.transform.position, randPos[currentTarget]);
            if (dist <= 0.5f) {
                currentTarget = (currentTarget + 1) % 2;
                MoveToLocation(randPos[currentTarget]);
            }
        } else {

        }
    }

    protected override void TargetedEntity(int param) {
        base.TargetedEntity(param);
    }
}
