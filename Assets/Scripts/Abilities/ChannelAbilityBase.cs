using RockUtils.GameEvents;

public class ChannelAbilityBase : AbilityBase {
    bool channeling = false;

    protected override string GetName() { return "ChannelAbilityBase"; }

    protected override TriggerType GetTriggerType() { return TriggerType.Channel; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Held + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Held + (int) abilityID, AttemptUseAbility);
    }

    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);

        interruptsMovement = true;
    }

    protected override bool CanUseAbility() {
        return true;
    }
}
