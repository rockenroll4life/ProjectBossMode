using UnityEngine;

public abstract class Locomotion {
    public enum MovementType {
        Mouse,
        Keyboard,
        Controller,
    }

    protected static readonly float ROTATION_SPEED = 720f;

    protected LivingEntity owner;
    protected float speed;

    public Locomotion(LivingEntity owner) {
        this.owner = owner;

        speed = owner.GetAttribute(AttributeTypes.MovementSpeed).GetValue();
        owner.GetAttributes().RegisterListener(AttributeTypes.MovementSpeed, SpeedChanged);
    }

    public abstract Vector3 GetMovementDirection();

    public abstract MovementType GetMovementType();

    public abstract bool IsMoving();

    protected abstract Vector3 GetLookingDirection();

    public abstract void StopMovement();


    public virtual void Update() {
        UpdateRotation();
        UpdateMovement();
    }

    void UpdateRotation() {
        Vector3 lookDir = GetLookingDirection();
        if (lookDir != Vector3.zero) {
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(lookDir), ROTATION_SPEED * Time.deltaTime);
        }
    }

    protected virtual void UpdateMovement() {
        owner.transform.position += GetMovementDirection() * speed * Time.deltaTime;
    }

    protected virtual void SpeedChanged(int param) {
        speed = param / 1000f;
    }
}
