using RockUtils.GameEvents;

public abstract class ToggleAbilityBase : AbilityBase {
    protected bool toggled = false;
    public ToggleAbilityBase(Player owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press, AbilityPressed);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Press, AbilityPressed);
    }

    protected override bool CanUseAbility() {
        return true;
    }

    protected override void UseAbility() {
        base.UseAbility();

        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Toggle, (int) GetAbilityBinding());
        toggled = !toggled;

        if (toggled) {
            owner.UseResource(GetResourceCost());
        }
    }

    void AbilityPressed(int param) {
        if (param == (int) GetAbilityBinding()) {
            AttemptUseAbility(param);
        }
    }
}
