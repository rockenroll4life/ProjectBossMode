using UnityEngine;

public interface Targeter {
    public IDamageable GetTargetedEntity();

    public Vector3? GetTargetedLocation();

    public void SetTargetedEntity(IDamageable entity);

    public void SetTargetedLocation(Vector3? location);

    public void Update();
}

