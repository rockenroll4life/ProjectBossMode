using RockUtils.GameEvents;

public abstract class ToggleAbilityBase : AbilityBase {
    protected bool toggled = false;
    public ToggleAbilityBase(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
    }

    protected override TriggerType GetTriggerType() => TriggerType.Toggle;

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press + GetAbilityID(), AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Press + GetAbilityID(), AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        return true;
    }

    protected override void UseAbility() {
        base.UseAbility();

        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Toggle + GetAbilityID());
        toggled = !toggled;
    }
}
