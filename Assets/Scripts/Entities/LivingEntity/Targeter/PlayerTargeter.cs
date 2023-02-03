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
        LivingEntity hitEntity = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            hitEntity = hit.collider.gameObject.GetComponentInParent<LivingEntity>();
            targetedLocation = null;

            if (hitEntity != null) {
                hitType = EntityTypeToTargetType(hitEntity.GetEntityType());
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_Entity);
                targetedLocation = null;
            } else {
                //  If we have an entity selected, let's go ahead and deselect it first. Don't just move if something is selected
                if (targetedEntity) {
                    targetedEntity.OnDeselected();
                    targetedEntity = null;
                } else {
                    hitType = TargetType.World;
                    targetedLocation = hit.point;
                    EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
                }
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
        if (hitEntity != targetedEntity) {
            if (targetedEntity == null) {
                targetedEntity = hitEntity;
                targetedEntity.OnSelected();
            } else if (hitEntity == null) {
                targetedEntity.OnDeselected();
                targetedEntity = null;
            } else {
                targetedEntity.OnDeselected();
                hitEntity.OnSelected();
                targetedEntity = hitEntity;
            }
        } else if (targetedEntity != null) {
            targetedEntity.OnDeselected();
            targetedEntity = null;
        }
    }

    void UpdateMoveLocation(int param) {
        if (hitType == TargetType.World) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //  TODO: [Rock]: We should probably use a layer mask in this instance to only collide with the world?
            Physics.Raycast(ray, out hit, RAYCAST_DISTANCE);
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
        }
    }
}
