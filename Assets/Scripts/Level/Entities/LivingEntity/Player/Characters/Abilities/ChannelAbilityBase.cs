using UnityEngine;
using RockUtils.GameEvents;

public abstract class ChannelAbilityBase : AbilityBase {
    public ChannelAbilityBase(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override TriggerType GetTriggerType() => TriggerType.Channel;
    //  TODO: [Rock]: Let's not make Channels cancel movement by default, additionally, we need to add a listener
    //  for entity or world selection to cancel the channel if this ability interrupts movement
    protected override bool InterruptsMovement() => true;

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press + GetAbilityID(), AbilityStart);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Release + GetAbilityID(), AbilityStop);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Held + GetAbilityID(), AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Held + GetAbilityID(), AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        ResourceCost resourceCost = GetResourceCost();
        EntityDataType resourceType = resourceCost.GetResourceType();

        return owner.GetEntityData(resourceType) > resourceCost.GetCost(owner) * Time.deltaTime;
    }

    protected override void UseAbility() {
        base.UseAbility();

        owner.UseResource(GetResourceCost(), Time.deltaTime);
    }

    protected virtual void AbilityStart(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Channel_Start + GetAbilityID());
    }

    protected virtual void AbilityStop(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Channel_Stop + GetAbilityID());
    }
}
