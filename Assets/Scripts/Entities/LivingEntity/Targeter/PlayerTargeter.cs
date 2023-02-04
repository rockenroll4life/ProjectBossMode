using UnityEngine;
using RockUtils.GameEvents;

public class PlayerTargeter : TargeterBase {
    LivingEntity targetedEntity = null;
    Vector3? targetedLocation = null;

    RaycastHit hit;
    TargetType hitType = TargetType.None;

    public PlayerTargeter(LivingEntity owner)
        : base(owner) {
        EventManager.StartListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
        EventManager.StartListening((int) GameEvents.Mouse_Left_Held, UpdateMoveLocation);
    }

    ~PlayerTargeter() {
        EventManager.StopListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
        EventManager.StopListening((int) GameEvents.Mouse_Left_Held, UpdateMoveLocation);
    }

    public override LivingEntity GetTargetedEntity() {
        return targetedEntity;
    }

    public override Vector3? GetTargetedLocation() {
        return targetedLocation;
    }

    void SelectTarget(int param) {
        LivingEntity prevTargetedEntity = targetedEntity;
        LivingEntity hitEntity = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            hitEntity = hit.collider.gameObject.GetComponentInParent<LivingEntity>();
            targetedLocation = null;

            if (hitEntity != null) {
                hitType = EntityTypeToTargetType(hitEntity.GetEntityType());
                targetedEntity = hitEntity;
                targetedLocation = null;
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_Entity);
            } else {
                hitType = TargetType.World;
                targetedEntity = null;
                targetedLocation = hit.point;
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
            }
        } else {
            //  NOTE: [Rock]: There shouldn't really ever be an instance where we don't collide with something in the future, however we'll keep this here for now
            hitType = TargetType.None;
            targetedLocation = null;

            if (targetedEntity) {
                targetedEntity.OnDeselected();
                targetedEntity = null;
            }
        }

        //  We either selected or unselected an entity
        if (hitEntity != prevTargetedEntity) {
            if (prevTargetedEntity == null) {
                if (hitEntity != owner) {
                    hitEntity.OnSelected();
                }
            } else if (hitEntity == null) {
                if (prevTargetedEntity != owner) {
                    prevTargetedEntity.OnDeselected();
                }
            } else {
                prevTargetedEntity.OnDeselected();
                hitEntity.OnSelected();
            }
        }
    }

    void UpdateMoveLocation(int param) {
        if (hitType == TargetType.World) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //  NOTE: [Rock]: In the future we'll probably want to have an ignore mask here...
            if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
                targetedLocation = hit.point;
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
            }
        }
    }
}
