using RockUtils.GameEvents;

public class ChannelAbilityBase : AbilityBase {
    public ChannelAbilityBase(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
        interruptsMovement = true;
    }

    protected override string GetName() { return "ChannelAbilityBase"; }

    protected override TriggerType GetTriggerType() { return TriggerType.Channel; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + (int) abilityID, AbilityStart);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Release + (int) abilityID, AbilityStop);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Held + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Ability_Held + (int) abilityID, AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        return true;
    }

    protected virtual void AbilityStart(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Channel_Start + (int) abilityID);
    }

    protected virtual void AbilityStop(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Channel_Stop + (int) abilityID);
    }
}