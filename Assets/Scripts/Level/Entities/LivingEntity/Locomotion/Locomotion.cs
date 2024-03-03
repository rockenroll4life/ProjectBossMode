using UnityEngine;
using RockUtils.GameEvents;

public abstract class Locomotion {
    public enum MovementType {
        Mouse,
        Keyboard,
        Controller,
    }

    public static readonly float FIXED_MOVEMENT_SPEED = 1.414f;
    protected static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;

    public Locomotion(LivingEntity owner) {
        this.owner = owner;
    }

    public abstract bool IsMoving();

    protected abstract Vector3 GetLookingDirection();

    public abstract void StopMovement();

    public virtual void Update() {
        UpdateRotation();
        UpdateMovement();
    }

    protected virtual void UpdateRotation() { }

    protected virtual void UpdateMovement() { }
}
