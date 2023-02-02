using UnityEngine;
using RockUtils.GameEvents;

public class TargetingManager {
    public enum TargetType {
        None,
        World,
        LivingEntity,
        Mob,
        Player,
        Interactable,
    }

    static readonly int RAYCAST_DISTANCE = 100;

    Entity owner;
    RaycastHit hit;
    bool validRaycastHit;
    TargetType hitType = TargetType.None;
    Entity targetedEntity = null;

    public bool IsValidHit(out RaycastHit hit) {
        hit = this.hit;
        return validRaycastHit;
    }

    public void Setup(Entity owner) {
        this.owner = owner;

        if (owner.GetEntityType() == Entity.EntityType.Player) {
            EventManager.StartListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
            EventManager.StartListening((int) GameEvents.Mouse_Left_Held, UpdateMoveLocation);
        }
    }

    public void Breakdown() {
        if (owner.GetEntityType() == Entity.EntityType.Player) {
            EventManager.StopListening((int) GameEvents.Mouse_Left_Press, SelectTarget);
            EventManager.StopListening((int) GameEvents.Mouse_Left_Held, UpdateMoveLocation);
        }
    }

    void SelectTarget(int param) {
        Entity hitEntity = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, RAYCAST_DISTANCE)) {
            validRaycastHit = true;
            hitEntity = hit.collider.gameObject.GetComponentInParent<Entity>();

            if (hitEntity != null) {
                hitType = hitEntity.GetTargetType();
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_Entity);
            } else {
                //  If we have an entity selected, let's go ahead and deselect it first. Don't just move if something is selected
                if (targetedEntity) {
                    targetedEntity.OnDeselected();
                    targetedEntity = null;
                } else {
                    hitType = TargetType.World;
                    EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
                }
            }
        } else {
            //  NOTE: [Rock]: There shouldn't really ever be an instance where we don't collide with something in the future, however we'll keep this here for now
            validRaycastHit = false;
            hitType = TargetType.None;
        }

        //  We either selected or unselected an entity
        if (hitEntity != targetedEntity ) {
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
