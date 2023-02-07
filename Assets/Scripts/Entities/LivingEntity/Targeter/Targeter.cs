using UnityEngine;

public interface Targeter {
    public LivingEntity GetTargetedEntity();

    public Vector3? GetTargetedLocation();

    public void SetTargetedEntity(LivingEntity entity);

    public void SetTargetedLocation(Vector3? location);

    public void Update();
}

