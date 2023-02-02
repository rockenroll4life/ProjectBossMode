using RockUtils.GameEvents;

public class ToggleAbilityBase : AbilityBase {
    protected bool toggled = false;
    public ToggleAbilityBase(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() { return "ToggleAbilityBase"; }

    protected override TriggerType GetTriggerType() {
        return TriggerType.Toggle;
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        return true;
    }

    protected override void UseAbility() {
        base.UseAbility();

        EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Toggle + (int) abilityID);
        toggled = !toggled;
    }
}
