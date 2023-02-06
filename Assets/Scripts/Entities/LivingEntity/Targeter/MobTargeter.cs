using UnityEngine;
using RockUtils.GameEvents;

public class MobTargeter : LivingEntityTargeter {
    //  TEMP
    readonly Vector3[] randPos = new Vector3[2];
    int currentTarget = 0;

    public MobTargeter(LivingEntity owner)
        : base(owner) {

        randPos[0] = new Vector3(-5, 0, 6);
        randPos[1] = new Vector3(5, 0, 6);

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.LivingEntity_Hurt, EntityHurt);
    }

    ~MobTargeter() {
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.LivingEntity_Hurt, EntityHurt);
    }

    //  TODO: [Rock]: Get the update function out of the targeter, we need to have AI behaviors tell the Targeter what to target (Entity or World) so it can then tell others
    public override void Update() {
        base.Update();

        if (targetedLocation == null) {
            targetedLocation = randPos[currentTarget];
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
        }

        //  NOTE: [Rock]: Code like this will want to be moved out of here and into the targeter...
        if (!targetedEntity) {
            float dist = Vector3.Distance(owner.transform.position, randPos[currentTarget]);
            if (dist <= 0.5f) {
                currentTarget = (currentTarget + 1) % 2;
                targetedLocation = randPos[currentTarget];
                EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_World);
            }
        }
    }

    //  NOTE: [Rock]: This will also want to be a part of a AI behavior
    void EntityHurt(int param) {
        Entity lastDamager = owner.GetLastDamager();
        if (lastDamager.GetEntityType() == Entity.EntityType.Player) {
            targetedEntity = (LivingEntity) owner.GetLastDamager();
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Targeted_Entity);
        }
    }
}
