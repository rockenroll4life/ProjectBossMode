using UnityEngine;

public interface Targeter {
    public LivingEntity GetTargetedEntity();

    public Vector3? GetTargetedLocation();
}

