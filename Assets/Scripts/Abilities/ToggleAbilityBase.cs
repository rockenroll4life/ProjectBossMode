using RockUtils.GameEvents;

public class ToggleAbilityBase : AbilityBase {
    protected bool toggled = false;

    protected override string GetName() { return "ToggleAbilityBase"; }

    protected override TriggerType GetTriggerType() {
        return TriggerType.Toggle;
    }

    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        return true;
    }

    protected override void UseAbility() {
        base.UseAbility();

        EventManager.TriggerEvent((int) GameEvents.Ability_Toggle + (int) abilityID);
        toggled = !toggled;
    }
}