using UnityEngine;

public class TargetingManager : MonoBehaviour {
    public enum TargetType {
        None,
        World,
        Mob,
        Player,
        Interactable,
    }

    static readonly string PLAYER_TAG = "Player";
    static readonly string MOB_TAG = "Mob";
    static readonly string INTERACTABLE_TAG = "Interactable";

    static TargetingManager targetingManager;
    public static TargetingManager instance {
        get {
            if (!targetingManager) {
                targetingManager = FindObjectOfType(typeof(TargetingManager)) as TargetingManager;

                if (!targetingManager) {
                    Debug.LogError("Using this requires an EventManager on a GameObject within the scene");
                }
            }
            return targetingManager;
        }
    }

    static readonly int RAYCAST_DISTANCE = 100;
    
    Camera cam;
    RaycastHit hit;
    bool validRaycastHit;
    TargetType hitType = TargetType.None;
    Entity targetedEntity = null;

    private void Start() {
        cam = Camera.main;
    }

    public static bool IsValidHit(out RaycastHit hit) {
        hit = instance.hit;
        return instance.validRaycastHit;
    }

    public static Vector3 GetHitLocation() {
        return instance.hit.point;
    }

    public static TargetType GetHitType() {
        return instance.hitType;
    }

    private void Update() {
        //  Once a frame we'll cast out a ray to see what we hit so that we can cache the value and don't have to cast more than once
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            validRaycastHit = true;

            Entity entity = hit.collider.gameObject.GetComponentInParent<Entity>();

            //  We stopped targeting what we previously were
            if (targetedEntity != entity && targetedEntity != null) {
                targetedEntity.OnStopHovering();
                targetedEntity = null;
            }

            //  We found a new target
            if (targetedEntity == null && entity != null) {
                targetedEntity = entity;
                targetedEntity.OnStartHovering();
            }

            if (entity != null) {
                if (entity.entityType == Entity.EntityType.Mob) {
                    hitType = TargetType.Mob;
                } else if (entity.entityType == Entity.EntityType.Player) {
                    hitType = TargetType.Player;
                } else if (entity.entityType == Entity.EntityType.Interactable) {
                    hitType = TargetType.Interactable;
                }
            } else {
                hitType = TargetType.World;
            }
        } else {
            validRaycastHit = false;
            hitType = TargetType.None;

            //  TODO: [Rock]: Should we ever really be able to target nothing? We should always at least hit the ground...Investigate
            if (targetedEntity != null) {
                targetedEntity.OnStopHovering();
                targetedEntity = null;
            }
        }
    }
}
