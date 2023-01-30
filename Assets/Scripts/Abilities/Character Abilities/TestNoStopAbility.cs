using RockUtils.GameEvents;

public class TestNoStopAbility : AbilityBase {
    public override void Setup(Entity owner, string name, float cooldownTime) {
        base.Setup(owner, name, cooldownTime);

        triggerType = TriggerType.Toggle;
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Press + abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Press + abilityID, AttemptUseAbility);
    }
}
