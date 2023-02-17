using UnityEngine;

public interface Targeter {
    public Damageable GetTargetedEntity();

    public Vector3? GetTargetedLocation();

    public void SetTargetedEntity(Damageable entity);

    public void SetTargetedLocation(Vector3? location);

    public void Update();
}

