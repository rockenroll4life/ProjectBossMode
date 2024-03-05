using UnityEngine;
using RockUtils.GameEvents;

public class PlayerMouseTargeter : TargeterBase {
    RaycastHit hit;
    TargetType hitType = TargetType.None;

    public PlayerMouseTargeter(LivingEntity owner)
        : base(owner) {
        EventManager.StartListening(GameEvents.Mouse_Left_Press, SelectTarget);
        EventManager.StartListening(GameEvents.Mouse_Left_Held, UpdateMoveLocation);
    }

    ~PlayerMouseTargeter() {
        EventManager.StopListening(GameEvents.Mouse_Left_Press, SelectTarget);
        EventManager.StopListening(GameEvents.Mouse_Left_Held, UpdateMoveLocation);
    }

    void SelectTarget(int param) {
        IDamageable prevTargetedEntity = targetedEntity;
        IDamageable hitEntity = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            hitEntity = hit.collider.gameObject.GetComponentInParent<IDamageable>();
            targetedLocation = null;

            if (hitEntity != null) {
                hitType = EntityTypeToTargetType(hitEntity.GetEntity().GetEntityType());
                targetedEntity = hitEntity;
                targetedLocation = null;
                EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Targeted_Entity);
            } else {
                hitType = TargetType.World;
                targetedEntity = null;
                targetedLocation = hit.point;
                EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Targeted_World);
            }
        } else {
            //  NOTE: [Rock]: There shouldn't really ever be an instance where we don't collide with something in the future, however we'll keep this here for now
            hitType = TargetType.None;
            targetedLocation = null;

            if (targetedEntity != null) {
                targetedEntity.GetEntity().OnDeselected();
                targetedEntity = null;
            }
        }

        //  We either selected or unselected an entity
        if (hitEntity != prevTargetedEntity) {
            if (prevTargetedEntity == null) {
                if (hitEntity.GetEntity() != owner) {
                    hitEntity.GetEntity().OnSelected();
                }
            } else if (hitEntity == null) {
                if (prevTargetedEntity.GetEntity() != owner) {
                    prevTargetedEntity.GetEntity().OnDeselected();
                }
            } else {
                prevTargetedEntity.GetEntity().OnDeselected();
                hitEntity.GetEntity().OnSelected();
            }
        }
    }

    void UpdateMoveLocation(int param) {
        if (hitType == TargetType.World) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //  NOTE: [Rock]: In the future we'll probably want to have an ignore mask here...
            if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
                targetedLocation = hit.point;
                EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Targeted_World);
            }
        }
    }
}
