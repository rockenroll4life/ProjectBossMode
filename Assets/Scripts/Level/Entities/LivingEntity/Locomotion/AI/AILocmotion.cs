using UnityEngine;

public class AILocmotion : Locomotion {
    public AILocmotion(LivingEntity owner)
        : base(owner) {
        this.owner = owner;
    }

    public override Vector3 GetMovementDirection() {
        return Vector3.zero;
    }

    public override MovementType GetMovementType() {
        return MovementType.AI;
    }

    public override bool IsMoving() {
        return false;
    }

    public override void StopMovement() {
        
    }

    protected override Vector3 GetLookingDirection() {
        return Vector3.zero;
    }
}
