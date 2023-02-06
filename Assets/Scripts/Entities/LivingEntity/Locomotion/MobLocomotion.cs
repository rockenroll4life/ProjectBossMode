using UnityEngine;

//  NOTE: [Rock]: ...We may no longer need a mob locomotion either...
public class MobLocomotion : LivingEntityLocomotion {
    public MobLocomotion(LivingEntity owner)
        : base(owner) {
    }
}
