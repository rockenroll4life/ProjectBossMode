using UnityEngine;
using RockUtils.GameEvents;

public class TargetingManager : MonoBehaviour {
    public enum TargetType {
        None,
        World,
        Mob,
        Player,
        Interactable,
    }

    static TargetingManager _instance;
    public static TargetingManager instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType<TargetingManager>();

                if (!_instance) {
                    Debug.LogError("Using this requires an EventManager on a GameObject within the scene");
                }
            }
            return _instance;
        }
    }

    static readonly int RAYCAST_DISTANCE = 100;

    RaycastHit hit;
    bool validRaycastHit;
    TargetType hitType = TargetType.None;
    Entity targetedEntity = null;

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

    private void Start() {
        EventManager.StartListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
    }

    private void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
    }

    void SelectTarget(int param) {
        Entity hitEntity = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            validRaycastHit = true;
            hitEntity = hit.collider.gameObject.GetComponentInParent<Entity>();

            if (hitEntity != null) {
                if (hitEntity.entityType == Entity.EntityType.Mob) {
                    hitType = TargetType.Mob;
                } else if (hitEntity.entityType == Entity.EntityType.Player) {
                    hitType = TargetType.Player;
                } else if (hitEntity.entityType == Entity.EntityType.Interactable) {
                    hitType = TargetType.Interactable;
                }
            } else {
                hitType = TargetType.World;
            }
        }

        //  We either selected or unselected an entity
        if (hitEntity != targetedEntity ) {
            if (targetedEntity == null) {
                targetedEntity = hitEntity;
                targetedEntity.OnStartHovering();
            } else if (hitEntity == null) {
                targetedEntity.OnStopHovering();
                targetedEntity = null;
            } else {
                targetedEntity.OnStopHovering();
                hitEntity.OnStartHovering();
                targetedEntity = hitEntity;
            }
        }
    }


    //  NOTE: [Rock]: For now we're going to not worry about ticking raycast to highlight an entity, while nice, it probably isn't what we want to do.
    //  For now we'll leave this here in case we want to approach this later in the future.
    /*private void Update() {
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
    }*/
}
