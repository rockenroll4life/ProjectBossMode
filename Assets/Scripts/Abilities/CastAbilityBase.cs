using RockUtils.GameEvents;

public class CastAbilityBase : AbilityBase
{
    protected override string GetName() { return "CastAbilityBase"; }

    protected override TriggerType GetTriggerType() { return TriggerType.Cast; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override bool CanUseAbility() {
        return /*cooldown.currentValue == 0 ||*/ canBypassCooldown();
    }
    protected override void UseAbility() {
        base.UseAbility();

        //cooldown.ResetCurrent();
    }
}
